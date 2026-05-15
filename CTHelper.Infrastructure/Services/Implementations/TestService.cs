using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.TestModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class TestService : ITestService
    {
        private readonly AppDbContext _dbContext;

        public TestService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<Test>> CreateMixedTest(CreateMixedTestRequestModel requestModel)
        {
            var allProblems = new List<Problem>();
            foreach (var topic in requestModel.TopicItems)
            {
                var topicProblems = await _dbContext.Problems
                    .Include(p => p.Versions.Where(v => v.IsActive))
                    .Where(p => p.TopicId == topic.TopicId
                                && !p.IsDeleted
                                && p.IsPublished
                                && p.Versions.Any(v => v.IsActive
                                                        && (int)v.Difficulty == (int)requestModel.AverageDifficult))
                    .OrderBy(r => Guid.NewGuid())
                    .Take((int)topic.ProblemCount)
                    .ToListAsync();
                allProblems.AddRange(topicProblems);
            }

            if (!allProblems.Any())
            {
                return new OperationResult<Test>
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "No problems found for selected parameters",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var singleChoiceProblems = allProblems
                .Where(p => p.Versions.FirstOrDefault(pv => pv.IsActive)!.Type == ProblemTypeEnum.SingleChoice)
                .ToList();

            var multipleChoiceProblems = allProblems
                .Where(p => p.Versions.FirstOrDefault(pv => pv.IsActive)!.Type == ProblemTypeEnum.MultipleChoice)
                .ToList();

            var openEndedProblems = allProblems
                .Where(p => p.Versions.FirstOrDefault(pv => pv.IsActive)!.Type == ProblemTypeEnum.OpenEnded)
                .ToList();

            var aTypeProblems = singleChoiceProblems
                .Concat(multipleChoiceProblems)
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            var bTypeProblems = openEndedProblems
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            var testProblems = new List<TestProblem>();

            int aCounter = 1;
            foreach (var problem in aTypeProblems)
            {
                testProblems.Add(new TestProblem
                {
                    ProblemId = problem.Id,
                    Code = $"A{aCounter}"
                });
                aCounter++;
            }

            int bCounter = 1;
            foreach (var problem in bTypeProblems)
            {
                testProblems.Add(new TestProblem
                {
                    ProblemId = problem.Id,
                    Code = $"B{bCounter}"
                });
                bCounter++;
            }

            testProblems = testProblems
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            var subject = await _dbContext.Subjects
                .Where(s => s.Id == requestModel.SubjectId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();

            var difficultyText = requestModel.AverageDifficult switch
            {
                ProblemDifficultEnum.VeryEasy => "очень легкий",
                ProblemDifficultEnum.Easy => "легкий",
                ProblemDifficultEnum.Normal => "средний",
                ProblemDifficultEnum.Hard => "сложный",
                ProblemDifficultEnum.VeryHard => "очень сложный",
                _ => "смешанный"
            };

            var testTitle = $"Смешанный тест по {subject ?? "предмету"} ({difficultyText}) от {DateTimeOffset.UtcNow:dd.MM.yyyy HH:mm}";

            var test = new Test
            {
                Title = testTitle,
                SubjectId = requestModel.SubjectId,
                AuthorId = requestModel.AuthorId,
                Type = TestTypeEnum.Mixed,
                IsTraning = true,
                IsPublished = true,
                IsPublic = false,
                Duration = 3600,
                AttemptsCount = 1,
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdateAt = DateTimeOffset.UtcNow,
                TestProblems = testProblems
            };

            await _dbContext.Tests.AddAsync(test);
            await _dbContext.SaveChangesAsync();

            return new OperationResult<Test>(test);
        }

        public async Task<OperationResult> CreateTest(CreateTestRequestModel requestModel)
        {
            var test = new Test
            {
                Title = requestModel.Title,
                SubjectId = requestModel.SubjectId,
                AuthorId = requestModel.AuthorId,
                Type = TestTypeEnum.Custom,
                IsTraning = requestModel.IsTraning,
                IsPublished = requestModel.IsPublished,
                IsPublic = requestModel.IsPublic,
                Duration = requestModel.Duration ?? 0,
                AttemptsCount = requestModel.AttemptsCount ?? 0,
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdateAt = DateTimeOffset.UtcNow,
                TestProblems = requestModel.TestProblemList.Select(x => new TestProblem
                {
                    ProblemId = x.ProblemId,
                    Code = x.Code
                }).ToList()
            };

            await _dbContext.Tests.AddAsync(test);
            await _dbContext.SaveChangesAsync();
            return new OperationResult();
        }

        public async Task<OperationResult<TestDetailsModel>> GetTestDetails(TestDetailsRequestModel requestModel)
        {
            var test = await _dbContext.Tests
                .Where(t => t.Id == requestModel.TestId
                            && !t.IsDeleted
                            && (t.AuthorId == requestModel.UserId || t.IsPublic))
                .AsNoTracking()
                .Select(t => new TestDetailsModel
                {
                    TestId = t.Id,
                    TestName = t.Title,
                    AuthorId = t.AuthorId,
                    AuthorName = t.Author.Username,
                    ProblemCount = t.TestProblems.Count,
                    Type = t.Type,
                    AttemptsLeft = t.AttemptsCount,
                    AvgDifficult = (ProblemDifficultEnum)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty),
                    Problems = t.TestProblems.Select(tp => new TestProblemModel
                    {
                        ProblemId = tp.ProblemId,
                        Code = tp.Code,
                        Type = tp.Problem.Versions.First(v => v.IsActive).Type,
                        Difficulty = tp.Problem.Versions.First(v => v.IsActive).Difficulty,
                        Statement = tp.Problem.Versions.First(v => v.IsActive).Statement,
                        Answer = tp.Problem.Versions.First(v => v.IsActive).CorrectAnswer,
                        Explanation = tp.Problem.Versions.First(v =>v.IsActive).Explanation,
                    })
                })
                .FirstOrDefaultAsync();

            if (test == null)
            {
                return new OperationResult<TestDetailsModel>
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Test not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            return new OperationResult<TestDetailsModel>(test);
        }

        public async Task<OperationResult<PaginatedListResponseModel<TestListItemModel>>> GetTestList(TeacherTestListRequestModel requestModel)
        {
            var query = _dbContext.Tests.Where(t => !t.IsDeleted);

            if (requestModel.OnlyMyTests.GetValueOrDefault())
            {
                query = query.Where(t => t.AuthorId == requestModel.UserId);
            }
            else
            {
                query = query.Where(t => t.AuthorId == requestModel.UserId || t.IsPublic == true);
            }

            if (!string.IsNullOrWhiteSpace(requestModel.NameFragment))
            {
                query = query.Where(t => t.Title.Contains(requestModel.NameFragment));
            }

            if (!string.IsNullOrWhiteSpace(requestModel.AuthorNameFragment))
            {
                query = query.Where(t => t.Author.Username.Contains(requestModel.AuthorNameFragment));
            }

            if (requestModel.AvgDifficult.HasValue)
            {
                query = query.Where(t => t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
                                        == (int)requestModel.AvgDifficult.Value);
            }

            if (requestModel.IsTraning.HasValue)
            {
                query = query.Where(t => t.IsTraning == requestModel.IsTraning.Value);
            }

            if (requestModel.Type.HasValue)
            {
                query = query.Where(t => t.Type == requestModel.Type.Value);
            }

            if (requestModel.MinTaskCount.HasValue)
            {
                query = query.Where(t => t.TestProblems.Count >= requestModel.MinTaskCount.Value);
            }

            if (requestModel.MaxTaskCount.HasValue)
            {
                query = query.Where(t => t.TestProblems.Count <= requestModel.MaxTaskCount.Value);
            }

            var totalCount = await query.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)totalCount / requestModel.PageSize);

            var tests = await query
                .AsNoTracking()
                .Select(t => new TestListItemModel
                {
                    TestId = t.Id,
                    TestName = t.Title,
                    AuthorName = t.Author.Username,
                    IsPublished = t.IsPublished,
                    ProblemCount = t.TestProblems.Count,
                    AvgDifficult = (ProblemDifficultEnum)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
                })
                .OrderByDescending(t => t.TestId)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<TestListItemModel>>(
                new PaginatedListResponseModel<TestListItemModel>
                {
                    Items = tests,
                    TotalPagesCount = pagesCount,
                    Page = requestModel.PageNumber,
                    PageSize = requestModel.PageSize,
                    HasPreviousPage = requestModel.PageNumber > 1,
                    HasNextPage = requestModel.PageNumber < pagesCount
                });
        }

        public async Task<OperationResult<PaginatedListResponseModel<TestListItemModel>>> GetTestList(MyTestListRequestModel requestModel)
        {
            var query = _dbContext.Tests.Where(t => !t.IsDeleted && t.AuthorId == requestModel.UserId);

            if (!string.IsNullOrWhiteSpace(requestModel.NameFragment))
            {
                query = query.Where(t => t.Title.Contains(requestModel.NameFragment));
            }

            if (requestModel.AvgDifficult.HasValue)
            {
                query = query.Where(t => t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
                                        == (int)requestModel.AvgDifficult.Value);
            }

            if (requestModel.IsTraning.HasValue)
            {
                query = query.Where(t => t.IsTraning == requestModel.IsTraning.Value);
            }

            if (requestModel.Type.HasValue)
            {
                query = query.Where(t => t.Type == requestModel.Type.Value);
            }

            if (requestModel.MinTaskCount.HasValue)
            {
                query = query.Where(t => t.TestProblems.Count >= requestModel.MinTaskCount.Value);
            }

            if (requestModel.MaxTaskCount.HasValue)
            {
                query = query.Where(t => t.TestProblems.Count <= requestModel.MaxTaskCount.Value);
            }

            var totalCount = await query.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)totalCount / requestModel.PageSize);

            var tests = await query
                .AsNoTracking()
                .Select(t => new TestListItemModel
                {
                    TestId = t.Id,
                    TestName = t.Title,
                    IsPublished = t.IsPublished,
                    AuthorName = t.Author.Username,
                    ProblemCount = t.TestProblems.Count,
                    AvgDifficult = (ProblemDifficultEnum)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
                })
                .OrderByDescending(t => t.TestId)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<TestListItemModel>>(
                new PaginatedListResponseModel<TestListItemModel>
                {
                    Items = tests,
                    TotalPagesCount = pagesCount,
                    Page = requestModel.PageNumber,
                    PageSize = requestModel.PageSize,
                    HasPreviousPage = requestModel.PageNumber > 1,
                    HasNextPage = requestModel.PageNumber < pagesCount
                });
        }

        public async Task<OperationResult<PaginatedListResponseModel<TestListItemModel>>> GetTestList(StudentTestListRequestModel requestModel)
        {
            var query = _dbContext.Tests.Where(t => !t.IsDeleted && (t.IsPublic || t.AuthorId == requestModel.UserId));

            if (!string.IsNullOrWhiteSpace(requestModel.NameFragment))
            {
                query = query.Where(t => t.Title.Contains(requestModel.NameFragment));
            }

            if (!string.IsNullOrWhiteSpace(requestModel.AuthorNameFragment))
            {
                query = query.Where(t => t.Author.Username.Contains(requestModel.AuthorNameFragment));
            }

            if (requestModel.AvgDifficult.HasValue)
            {
                query = query.Where(t => t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
                                        == (int)requestModel.AvgDifficult.Value);
            }

            if (requestModel.IsTraning.HasValue)
            {
                query = query.Where(t => t.IsTraning == requestModel.IsTraning.Value);
            }

            if (requestModel.Type.HasValue)
            {
                query = query.Where(t => t.Type == requestModel.Type.Value);
            }

            if (requestModel.MinTaskCount.HasValue)
            {
                query = query.Where(t => t.TestProblems.Count >= requestModel.MinTaskCount.Value);
            }

            if (requestModel.MaxTaskCount.HasValue)
            {
                query = query.Where(t => t.TestProblems.Count <= requestModel.MaxTaskCount.Value);
            }

            if (requestModel.AssignedToMe.GetValueOrDefault())
            {
                query = query.Where(t => t.StudentAssignments.Any(sa => sa.StudentId == requestModel.UserId));
            }

            var totalCount = await query.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)totalCount / requestModel.PageSize);

            var tests = await query
                .AsNoTracking()
                .Select(t => new TestListItemModel
                {
                    TestId = t.Id,
                    TestName = t.Title,
                    AuthorName = t.Author.Username,
                    ProblemCount = t.TestProblems.Count,
                    AvgDifficult = (ProblemDifficultEnum)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
                })
                .OrderByDescending(t => t.TestId)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<TestListItemModel>>(
                new PaginatedListResponseModel<TestListItemModel>
                {
                    Items = tests,
                    TotalPagesCount = pagesCount,
                    Page = requestModel.PageNumber,
                    PageSize = requestModel.PageSize,
                    HasPreviousPage = requestModel.PageNumber > 1,
                    HasNextPage = requestModel.PageNumber < pagesCount
                });
        }

        public async Task<OperationResult<TestPreviewModel>> GetTestPreview(TestPreviewRequestModel requestModel)
        {
            var test = await _dbContext.Tests
                .Where(t => t.Id == requestModel.TestId
                            && !t.IsDeleted
                            && (t.IsPublic || t.AuthorId == requestModel.UserId || t.StudentAssignments.Any(sa => sa.StudentId == requestModel.UserId)))
                .AsNoTracking()
                .Select(t => new TestPreviewModel
                {
                    TestId = t.Id,
                    TestName = t.Title,
                    AuthorId = t.AuthorId,
                    AuthorName = t.Author.Username,
                    ProblemCount = t.TestProblems.Count,
                    Type = t.Type,
                    AttemptsLeft = (int?)t.StudentAssignments
                        .Where(sa => sa.StudentId == requestModel.UserId && sa.TestId == t.Id)
                        .Select(sa => sa.AttemptsLeft)
                        .FirstOrDefault(),
                    AvgDifficult = (ProblemDifficultEnum)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
                })
                .FirstOrDefaultAsync();

            if (test == null)
            {
                return new OperationResult<TestPreviewModel>
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Test not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            return new OperationResult<TestPreviewModel>(test);
        }

        public async Task<OperationResult> RemoveTest(RemoveTestRequestModel requestModel)
        {
            var test = await _dbContext.Tests
                .FirstOrDefaultAsync(t => t.Id == requestModel.TestId
                                          && !t.IsDeleted
                                          && t.AuthorId == requestModel.UserId);

            if (test == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Test not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            test.IsDeleted = true;
            test.LastUpdateAt = DateTimeOffset.UtcNow;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> UpdateTest(UpdateTestRequestModel requestModel)
        {
            var test = await _dbContext.Tests
                .Include(t => t.TestProblems)
                .FirstOrDefaultAsync(t => t.Id == requestModel.TestId
                                          && !t.IsDeleted
                                          && t.AuthorId == requestModel.UserId);

            if (test == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Test not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            test.Title = requestModel.Title;
            test.IsTraning = requestModel.IsTraning;
            test.IsPublished = requestModel.IsPublished;
            test.IsPublic = requestModel.IsPublic;
            test.Duration = requestModel.Duration ?? 0;
            test.AttemptsCount = requestModel.AttemptsCount ?? 0;
            test.LastUpdateAt = DateTimeOffset.UtcNow;

            test.TestProblems.Clear();
            foreach (var problem in requestModel.TestProblemIdList)
            {
                test.TestProblems.Add(new TestProblem
                {
                    ProblemId = problem.ProblemId,
                    Code = problem.Code
                });
            }

            await _dbContext.SaveChangesAsync();
            return new OperationResult();
        }
    }
}