using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.TestAttemptModels;
using CTHelper.Application.Models.TestModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class TestAttemptService : ITestAttemptService
    {
        private readonly AppDbContext _dbContext;

        public TestAttemptService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> CancelTestAttempt(CancelTestAttemptRequestModel requestModel)
        {
            var attempt = await _dbContext.TestAttempts
                .FirstOrDefaultAsync(ta =>
                    ta.Id == requestModel.AttemptId
                    && ta.StudentId == requestModel.UserId);

            if (attempt == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotFound,
                    ErrorMessage = "Attempt not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (attempt.Status != TestAttemptStatusTypeEnum.InProgress && attempt.Status != TestAttemptStatusTypeEnum.Paused)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotActive,
                    ErrorMessage = "Attempt can not be cancelled",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            attempt.Status = TestAttemptStatusTypeEnum.Canceled;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }
        public async Task<OperationResult<CompleteTestAttemptResponseModel>> CompleteTestAttempt(CompleteTestAttemptRequestModel requestModel)
        {
            var attempt = await _dbContext.TestAttempts
                .Where(ta => ta.Id == requestModel.AttemptId && ta.StudentId == requestModel.UserId)
                .Select(ta => new
                {
                    ta.Id,
                    ta.Status,
                    ta.Duration,
                    ta.LastResumedAt,
                    ta.TestId,
                    IsTraining = ta.Test.IsTraning,
                    TestProblems = ta.Test.TestProblems.Select(tp => new
                    {
                        tp.Id,
                        tp.Code,
                        ProblemVersion = tp.Problem.Versions.FirstOrDefault(pv => pv.IsActive)
                    })
                })
                .FirstOrDefaultAsync();

            if (attempt == null)
            {
                return new OperationResult<CompleteTestAttemptResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotFound,
                    ErrorMessage = "Attempt not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (attempt.Status != TestAttemptStatusTypeEnum.InProgress && attempt.Status != TestAttemptStatusTypeEnum.Paused)
            {
                return new OperationResult<CompleteTestAttemptResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotActive,
                    ErrorMessage = "Attempt can not be completed",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            if (requestModel.UserAnswers != null && requestModel.UserAnswers.Any())
            {
                foreach (var userAnswerDto in requestModel.UserAnswers)
                {
                    var userAnswer = await _dbContext.UserAnswers
                        .FirstOrDefaultAsync(ua => ua.Id == userAnswerDto.UserAnswerId && ua.TestAttemptId == requestModel.AttemptId);

                    if (userAnswer != null)
                    {
                        userAnswer.Answer = userAnswerDto.Answer ?? string.Empty;
                    }
                }
                await _dbContext.SaveChangesAsync();
            }

            var userAnswers = await _dbContext.UserAnswers
                .Where(ua => ua.TestAttemptId == requestModel.AttemptId)
                .ToListAsync();

            foreach (var userAnswer in userAnswers)
            {
                var testProblem = attempt.TestProblems.FirstOrDefault(tp => tp.ProblemVersion?.Id == userAnswer.ProblemVersionId);
                if (testProblem?.ProblemVersion != null)
                {
                    userAnswer.IsCorrect = string.Equals(userAnswer.Answer?.Trim(), testProblem.ProblemVersion.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);
                }
            }

            var correctAnswers = userAnswers.Count(ua => ua.IsCorrect);
            var totalAnswers = userAnswers.Count;
            short? rawScore = totalAnswers > 0 ? (short)((double)correctAnswers / totalAnswers * 100) : (short)0;

            var attemptToUpdate = await _dbContext.TestAttempts
                .FirstOrDefaultAsync(ta => ta.Id == requestModel.AttemptId);

            if (attemptToUpdate != null)
            {
                if (attempt.Status == TestAttemptStatusTypeEnum.InProgress)
                {
                    attemptToUpdate.Duration += (int)Math.Ceiling((DateTimeOffset.UtcNow - attempt.LastResumedAt).TotalSeconds);
                }

                attemptToUpdate.RawScore = rawScore;
                attemptToUpdate.Status = TestAttemptStatusTypeEnum.Completed;
                await _dbContext.SaveChangesAsync();
            }

            var response = new CompleteTestAttemptResponseModel();
            if (attempt.IsTraining == false)
            {
                response.AttemptId = attempt.Id;
            }

            return new OperationResult<CompleteTestAttemptResponseModel>(response);
        }
        public async Task<OperationResult<TestAttemptDetails>> GetMyAttempt(MyTestAttemptRequestModel requestModel)
        {
            var attempt = await _dbContext.TestAttempts
                .Where(ta =>
                    ta.Id == requestModel.AttemptId
                    && ta.StudentId == requestModel.UserId)
                .AsNoTracking()
                .Select(ta => new TestAttemptDetails
                {
                    TestAttemptId = ta.Id,
                    TestName = ta.Test.Title,
                    TestId = ta.TestId,
                    StudentId = ta.StudentId,
                    StudentName = ta.Student.Username,
                    Status = ta.Status,
                    Duration = ta.Duration,
                    RawScore = ta.RawScore,
                    CreatedAt = ta.CreatedAt,
                    UserAnswers = ta.UserAnswers.Select(ua => new UserAnswerModel
                    {
                        ProblemId = ua.ProblemVersion.ProblemId,
                        IsActualProblemVersion = ua.ProblemVersion.IsActive,
                        Statement = ua.ProblemVersion.Statement,
                        Answer = ua.Answer,
                        IsCorrect = ua.IsCorrect,
                        CorrectAnswer = ua.ProblemVersion.CorrectAnswer,
                        Explanation = ua.ProblemVersion.Explanation,
                        Type = ua.ProblemVersion.Type,
                        Difficulty = ua.ProblemVersion.Difficulty,
                        TopicName = ua.ProblemVersion.Problem.Topic.Name
                    })
                })
                .FirstOrDefaultAsync();

            if (attempt == null)
            {
                return new OperationResult<TestAttemptDetails>
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotFound,
                    ErrorMessage = "Attempt not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            return new OperationResult<TestAttemptDetails>(attempt);
        }

        public async Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetMyAttemptList(MyTestAttemptListRequestModel requestModel)
        {
            var attemptsQuery = _dbContext.TestAttempts
                .Where(ta => ta.StudentId == requestModel.UserId)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(requestModel.TestNameFragment))
            {
                attemptsQuery = attemptsQuery.Where(ta => ta.Test.Title.StartsWith(requestModel.TestNameFragment));
            }

            var attemptsCount = await attemptsQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)attemptsCount / requestModel.PageSize);

            var attemptPageList = await attemptsQuery
                .Select(ta => new TestAttemptListItemModel
                {
                    TestAttemptId = ta.Id,
                    Status = ta.Status,
                    TestName = ta.Test.Title,
                    Duration = ta.Duration,
                    RawScore = ta.RawScore,
                    CreatedAt = ta.CreatedAt
                })
                .OrderByDescending(ta => ta.CreatedAt)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .ToListAsync();

            var paginatedList = new PaginatedListResponseModel<TestAttemptListItemModel>
            {
                Items = attemptPageList,
                TotalPagesCount = pagesCount,
                Page = requestModel.PageNumber,
                PageSize = requestModel.PageSize,
                HasPreviousPage = requestModel.PageNumber > 1,
                HasNextPage = requestModel.PageNumber < pagesCount
            };

            return new OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>(paginatedList);
        }

        public async Task<OperationResult<TestAttemptDetails>> GetStudentAttempt(StudentTestAttemptRequestModel requestModel)
        {
            var hasBinding = await _dbContext.TeacherStudents
                .Where(ts => ts.TeacherId == requestModel.UserId && ts.StudentId == requestModel.StudentId && !ts.IsDeleted)
                .AnyAsync();

            if (!hasBinding)
            {
                return new OperationResult<TestAttemptDetails>
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You do not have access to this student's attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var attempt = await _dbContext.TestAttempts
                .Where(ta =>
                    ta.Id == requestModel.AttemptId
                    && ta.StudentId == requestModel.StudentId)
                .AsNoTracking()
                .Select(ta => new TestAttemptDetails
                {
                    TestAttemptId = ta.Id,
                    TestName = ta.Test.Title,
                    TestId = ta.TestId,
                    StudentId = ta.StudentId,
                    StudentName = ta.Student.Username,
                    Status = ta.Status,
                    Duration = ta.Duration,
                    RawScore = ta.RawScore,
                    CreatedAt = ta.CreatedAt,
                    UserAnswers = ta.UserAnswers.Select(ua => new UserAnswerModel
                    {
                        ProblemId = ua.ProblemVersion.ProblemId,
                        IsActualProblemVersion = ua.ProblemVersion.IsActive,
                        Statement = ua.ProblemVersion.Statement,
                        Answer = ua.Answer,
                        IsCorrect = ua.IsCorrect,
                        CorrectAnswer = ua.ProblemVersion.CorrectAnswer,
                        Explanation = ua.ProblemVersion.Explanation,
                        Type = ua.ProblemVersion.Type,
                        Difficulty = ua.ProblemVersion.Difficulty,
                        TopicName = ua.ProblemVersion.Problem.Topic.Name
                    })
                })
                .FirstOrDefaultAsync();

            if (attempt == null)
            {
                return new OperationResult<TestAttemptDetails>
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotFound,
                    ErrorMessage = "Attempt not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            return new OperationResult<TestAttemptDetails>(attempt);
        }

        public async Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetStudentAttemptList(StudentTestAttemptListRequestModel requestModel)
        {
            var hasBinding = await _dbContext.TeacherStudents
                .Where(ts => ts.TeacherId == requestModel.UserId && ts.StudentId == requestModel.StudentId && !ts.IsDeleted)
                .AnyAsync();

            if (!hasBinding)
            {
                return new OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You do not have access to this student's attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var attemptsQuery = _dbContext.TestAttempts
                .Where(ta => ta.StudentId == requestModel.StudentId)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(requestModel.TestNameFragment))
            {
                attemptsQuery = attemptsQuery.Where(ta => ta.Test.Title.StartsWith(requestModel.TestNameFragment));
            }

            var attemptsCount = await attemptsQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)attemptsCount / requestModel.PageSize);

            var attemptPageList = await attemptsQuery
                .Select(ta => new TestAttemptListItemModel
                {
                    TestAttemptId = ta.Id,
                    Status = ta.Status,
                    TestName = ta.Test.Title,
                    Duration = ta.Duration,
                    RawScore = ta.RawScore,
                    CreatedAt = ta.CreatedAt
                })
                .OrderByDescending(ta => ta.CreatedAt)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .ToListAsync();

            var paginatedList = new PaginatedListResponseModel<TestAttemptListItemModel>
            {
                Items = attemptPageList,
                TotalPagesCount = pagesCount,
                Page = requestModel.PageNumber,
                PageSize = requestModel.PageSize,
                HasPreviousPage = requestModel.PageNumber > 1,
                HasNextPage = requestModel.PageNumber < pagesCount
            };

            return new OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>(paginatedList);
        }

        public async Task<OperationResult> PauseTestAttempt(PauseTestAttemptRequestModel requestModel)
        {
            var attempt = await _dbContext.TestAttempts
                .Where(ta => ta.Id == requestModel.AttemptId && ta.StudentId == requestModel.UserId)
                .Select(ta => new
                {
                    ta.Id,
                    ta.Status,
                    ta.Duration,
                    ta.LastResumedAt,
                    ta.TestId,
                    IsTraining = ta.Test.IsTraning
                })
                .FirstOrDefaultAsync();

            if (attempt == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotFound,
                    ErrorMessage = "Attempt not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (attempt.Status != TestAttemptStatusTypeEnum.InProgress)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotActive,
                    ErrorMessage = "Attempt is not in progress. You can pause only active attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            if (attempt.IsTraining == false)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptIsExaminative,
                    ErrorMessage = "You cannot pause examinative attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            if (requestModel.UserAnswers != null && requestModel.UserAnswers.Any())
            {
                foreach (var userAnswerDto in requestModel.UserAnswers)
                {
                    var userAnswer = await _dbContext.UserAnswers
                        .FirstOrDefaultAsync(ua => ua.Id == userAnswerDto.UserAnswerId && ua.TestAttemptId == requestModel.AttemptId);

                    if (userAnswer != null)
                    {
                        userAnswer.Answer = userAnswerDto.Answer ?? string.Empty;
                    }
                }
            }

            var attemptToUpdate = await _dbContext.TestAttempts
                .FirstOrDefaultAsync(ta => ta.Id == requestModel.AttemptId);

            if (attemptToUpdate != null)
            {
                attemptToUpdate.Duration += (int)Math.Ceiling((DateTimeOffset.UtcNow - attempt.LastResumedAt).TotalSeconds);
                attemptToUpdate.Status = TestAttemptStatusTypeEnum.Paused;
                await _dbContext.SaveChangesAsync();
            }

            return new OperationResult();
        }
        public async Task<OperationResult<TestPassingResponseModel>> ResumeTestAttempt(ResumeTestAttemptRequestModel requestModel)
        {
            var attempt = await _dbContext.TestAttempts
                .FirstOrDefaultAsync(ta => ta.Id == requestModel.AttemptId && ta.StudentId == requestModel.UserId);

            if (attempt == null)
            {
                return new OperationResult<TestPassingResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotFound,
                    ErrorMessage = "Attempt not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (attempt.Status != TestAttemptStatusTypeEnum.Paused)
            {
                return new OperationResult<TestPassingResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotActive,
                    ErrorMessage = "Attempt is not paused. You can resume only paused attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            attempt.Status = TestAttemptStatusTypeEnum.InProgress;
            attempt.LastResumedAt = DateTimeOffset.UtcNow;
            await _dbContext.SaveChangesAsync();

            var response = await BuildTestPassingResponse(attempt.Id, requestModel.UserId);
            return new OperationResult<TestPassingResponseModel>(response);
        }

        public async Task<OperationResult<TestPassingResponseModel>> StartTestAttempt(StartTestAttemptRequestModel requestModel)
        {
            var testInfo = await _dbContext.Tests
                .Where(t => t.Id == requestModel.TestId && t.IsPublished)
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.IsPublic,
                    t.AuthorId,
                    t.IsTraning,
                    t.Duration,
                    Assignment = t.StudentAssignments
                        .FirstOrDefault(sa => sa.StudentId == requestModel.UserId && sa.AttemptsLeft > 0)
                })
                .FirstOrDefaultAsync();

            if (testInfo == null)
            {
                return new OperationResult<TestPassingResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Test not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var hasAccess = testInfo.IsPublic || testInfo.Assignment != null || testInfo.AuthorId == requestModel.UserId;
            if (!hasAccess)
            {
                return new OperationResult<TestPassingResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You do not have access to this test",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var existingActiveAttempt = await _dbContext.TestAttempts
                .AnyAsync(ta => ta.TestId == requestModel.TestId
                             && ta.StudentId == requestModel.UserId
                             && ta.Status == TestAttemptStatusTypeEnum.InProgress);

            if (existingActiveAttempt)
            {
                return new OperationResult<TestPassingResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.AttemptAlreadyActive,
                    ErrorMessage = "You already have an active attempt for this test",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var newTestAttempt = new TestAttempt
                {
                    TestId = requestModel.TestId,
                    StudentId = requestModel.UserId,
                    Duration = 0,
                    Status = TestAttemptStatusTypeEnum.InProgress,
                    LastResumedAt = DateTimeOffset.UtcNow,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                await _dbContext.TestAttempts.AddAsync(newTestAttempt);
                await _dbContext.SaveChangesAsync();

                var testProblems = await _dbContext.TestProblems
                    .Include(tp => tp.Problem)
                        .ThenInclude(p => p.Versions)
                    .Where(tp => tp.TestId == requestModel.TestId)
                    .OrderBy(tp => tp.Code)
                    .ToListAsync();

                var userAnswers = new List<UserAnswer>();
                foreach (var testProblem in testProblems)
                {
                    var activeVersion = testProblem.Problem?.Versions.FirstOrDefault(pv => pv.IsActive);
                    if (activeVersion != null)
                    {
                        userAnswers.Add(new UserAnswer
                        {
                            TestAttemptId = newTestAttempt.Id,
                            ProblemVersionId = activeVersion.Id,
                            Answer = string.Empty,
                            IsCorrect = false,
                            CreatedAt = DateTimeOffset.UtcNow
                        });
                    }
                }

                await _dbContext.UserAnswers.AddRangeAsync(userAnswers);
                await _dbContext.SaveChangesAsync();

                if (testInfo.Assignment != null)
                {
                    testInfo.Assignment.AttemptsLeft--;
                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                var response = await BuildTestPassingResponse(newTestAttempt.Id, requestModel.UserId);
                return new OperationResult<TestPassingResponseModel>(response);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<TestPassingResponseModel> BuildTestPassingResponse(long attemptId, long userId)
        {
            var attempt = await _dbContext.TestAttempts
                .Where(ta => ta.Id == attemptId && ta.StudentId == userId)
                .Select(ta => new
                {
                    ta.TestId,
                    TestName = ta.Test.Title,
                    ta.Id,
                    ta.Status,
                    ta.Duration,
                    ta.RawScore,
                    TestProblems = ta.Test.TestProblems
                        .OrderBy(tp => tp.Code)
                        .Select(tp => new
                        {
                            tp.Code,
                            ProblemVersion = tp.Problem.Versions.FirstOrDefault(pv => pv.IsActive),
                            UserAnswer = ta.UserAnswers.FirstOrDefault(ua => ua.ProblemVersionId == (tp.Problem.Versions.FirstOrDefault(pv => pv.IsActive) != null ? tp.Problem.Versions.FirstOrDefault(pv => pv.IsActive).Id : 0))
                        })
                })
                .FirstOrDefaultAsync();

            if (attempt == null)
            {
                throw new InvalidOperationException("Attempt not found");
            }

            var problems = new List<TestPassingProblemModel>();

            foreach (var tp in attempt.TestProblems)
            {
                if (tp.ProblemVersion == null) continue;

                problems.Add(new TestPassingProblemModel
                {
                    Code = tp.Code,
                    Type = tp.ProblemVersion.Type,
                    Statement = tp.ProblemVersion.Statement,
                    UserAnswer = tp.UserAnswer?.Answer,
                    UserAnswerId = tp.UserAnswer?.Id ?? 0
                });
            }

            return new TestPassingResponseModel
            {
                TestId = attempt.TestId,
                TestName = attempt.TestName,
                AttemptId = attempt.Id,
                Status = attempt.Status,
                Duration = attempt.Duration,
                RawScore = attempt.RawScore,
                Problems = problems
            };
        }
    }
}
