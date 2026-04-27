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
            var problems = new List<Problem>();
            foreach (var topic in requestModel.TopicItems)
            {
                var topicProblems = await _dbContext.Problems
                    .Where(p => p.TopicId == topic.TopicId
                                && !p.IsDeleted
                                && p.IsPublished
                                && p.Versions.Any(v => v.IsActive
                                                    && (int)v.Difficulty == (int)requestModel.AverageDifficult))
                    .OrderBy(r => Guid.NewGuid())
                    .Take((int)topic.ProblemCount)
                    .ToListAsync();
                problems.AddRange(topicProblems);
            }

            if (!problems.Any())
            {
                return new OperationResult<Test>
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "No problems found for selected parameters",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var test = new Test
            {
                Title = "Сгенерированный тест",
                SubjectId = requestModel.SubjectId,
                AuthorId = requestModel.AuthorId,
                Type = TestType.Mixed,
                IsTraning = true,
                IsPublished = true,
                IsPublic = false,
                Duration = 3600,
                AttemptsCount = 1,
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdateAt = DateTimeOffset.UtcNow,
                TestProblems = problems.Select(p => new TestProblem
                {
                    ProblemId = p.Id,
                    Code = Guid.NewGuid().ToString("N").Substring(0, 8)
                }).ToList()
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
                Type = TestType.Custom,
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
                            && t.AuthorId == requestModel.UserId)
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
                    AvgDifficult = (ProblemDifficult)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty),
                    Problems = t.TestProblems.Select(tp => new TestProblemModel
                    {
                        ProblemId = tp.ProblemId,
                        Code = tp.Code,
                        Type = tp.Problem.Versions.First(v => v.IsActive).Type,
                        Difficulty = tp.Problem.Versions.First(v => v.IsActive).Difficulty,
                        Statement = tp.Problem.Versions.First(v => v.IsActive).Statement
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

            if (!string.IsNullOrWhiteSpace(requestModel.NameFragment))
            {
                query = query.Where(t => t.Title.StartsWith(requestModel.NameFragment));
            }

            if (!string.IsNullOrWhiteSpace(requestModel.AuthorNameFragment))
            {
                query = query.Where(t => t.Author.Username.StartsWith(requestModel.AuthorNameFragment));
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
                    ProblemCount = t.TestProblems.Count,
                    AvgDifficult = (ProblemDifficult)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
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
                query = query.Where(t => t.Title.StartsWith(requestModel.NameFragment));
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
                    ProblemCount = t.TestProblems.Count,
                    AvgDifficult = (ProblemDifficult)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
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
            var query = _dbContext.Tests.Where(t => !t.IsDeleted && t.IsPublished);

            if (!string.IsNullOrWhiteSpace(requestModel.NameFragment))
            {
                query = query.Where(t => t.Title.StartsWith(requestModel.NameFragment));
            }

            if (!string.IsNullOrWhiteSpace(requestModel.AuthorNameFragment))
            {
                query = query.Where(t => t.Author.Username.StartsWith(requestModel.AuthorNameFragment));
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
                query = query.Where(t => t.StudentAssignments.Any(sa => sa.StudentId == requestModel.UserId)
                                         || t.GroupAssignments.Any(ga => ga.Group.Students.Any(s => s.Id == requestModel.UserId)));
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
                    AvgDifficult = (ProblemDifficult)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty)
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

        public async Task<OperationResult<TestDetailsModel>> GetTestPreview(TestPreviewRequestModel requestModel)
        {
            var test = await _dbContext.Tests
                .Where(t => t.Id == requestModel.TestId
                            && !t.IsDeleted
                            && t.IsPublished)
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
                    AvgDifficult = (ProblemDifficult)t.TestProblems.Average(tp => (int)tp.Problem.Versions.First(v => v.IsActive).Difficulty),
                    Problems = t.TestProblems.Select(tp => new TestProblemModel
                    {
                        ProblemId = tp.ProblemId,
                        Code = tp.Code,
                        Type = tp.Problem.Versions.First(v => v.IsActive).Type,
                        Difficulty = tp.Problem.Versions.First(v => v.IsActive).Difficulty,
                        Statement = tp.Problem.Versions.First(v => v.IsActive).Statement
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