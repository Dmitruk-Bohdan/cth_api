using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.TestAttemptModels;
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

            if (attempt.Status != TestAttemptStatusType.InProgress && attempt.Status != TestAttemptStatusType.Paused)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotActive,
                    ErrorMessage = "Attempt can not be cancelled",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            attempt.Status = TestAttemptStatusType.Canceled;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> CompleteTestAttempt(CompleteTestAttemptRequestModel requestModel)
        {
            var attempt = await _dbContext.TestAttempts
                .Include(ta => ta.UserAnswers)
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

            if (attempt.Status != TestAttemptStatusType.InProgress && attempt.Status != TestAttemptStatusType.Paused)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotActive,
                    ErrorMessage = "Attempt can not be completed",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            if (attempt.Status == TestAttemptStatusType.InProgress)
            {
                attempt.Duration += (int)Math.Ceiling((DateTimeOffset.UtcNow - attempt.LastResumedAt).TotalSeconds);
            }

            var correctAnswers = attempt.UserAnswers.Count(ua => ua.IsCorrect);
            var totalAnswers = attempt.UserAnswers.Count;
            attempt.RawScore = totalAnswers > 0 ? (short)((double)correctAnswers / totalAnswers * 100) : (short)0;

            attempt.Status = TestAttemptStatusType.Completed;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
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
            var attemptInfo = await _dbContext.TestAttempts
                .Where(ta =>
                    ta.Id == requestModel.AttemptId
                    && ta.StudentId == requestModel.UserId)
                .Select(ta =>
                new
                {
                    ta.Status,
                    ta.TestId,
                    ta.StudentId,
                    IsExam = ta.Test.IsTraning
                })
                .FirstOrDefaultAsync();


            if(attemptInfo == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotFound,
                    ErrorMessage = "Attempt not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }
            if (attemptInfo.Status != TestAttemptStatusType.InProgress)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotActive,
                    ErrorMessage = "Attempt is not in progress. You can stop only active attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }
            if (attemptInfo.IsExam)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptIsExaminative,
                    ErrorMessage = "You can not stop examinative attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }


            var attempt = await _dbContext.TestAttempts.FirstOrDefaultAsync(ta => ta.Id == requestModel.AttemptId);

            attempt!.Duration += (int)Math.Ceiling((DateTimeOffset.UtcNow - attempt.LastResumedAt).TotalSeconds);

            attempt.Status = TestAttemptStatusType.Paused;

            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> ResumeTestAttempt(ResumeTestAttemptRequestModel requestModel)
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

            if (attempt.Status != TestAttemptStatusType.Paused)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotActive,
                    ErrorMessage = "Attempt is not paused. You can resume only paused attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            attempt.Status = TestAttemptStatusType.InProgress;
            attempt.LastResumedAt = DateTimeOffset.UtcNow;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> StartTestAttempt(StartTestAttemptRequestModel requestModel)
        {
            var testInfo = await _dbContext.Tests
                .Where(t => t.Id == requestModel.TestId && t.IsPublished)
                .Select(t => new
                {
                    t.IsPublic,
                    Assignment = t.StudentAssignments
                        .FirstOrDefault(sa => sa.StudentId == requestModel.UserId && sa.AttemptsLeft > 0)
                })
                .FirstOrDefaultAsync();

            if (testInfo == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Test not found",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var hasAccess = testInfo.IsPublic || testInfo.Assignment != null;
            if (!hasAccess)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var existingActiveAttempt = await _dbContext.TestAttempts
                .AnyAsync(ta => ta.TestId == requestModel.TestId
                             && ta.StudentId == requestModel.UserId
                             && ta.Status == TestAttemptStatusType.InProgress);

            if (existingActiveAttempt)
            {
                return new OperationResult
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
                    Status = TestAttemptStatusType.InProgress,
                    LastResumedAt = DateTimeOffset.UtcNow
                };
                await _dbContext.TestAttempts.AddAsync(newTestAttempt);

                if (testInfo.Assignment != null)
                {
                    testInfo.Assignment.AttemptsLeft--;
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new OperationResult();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
