using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Enums;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class ProblemService : IProblemService
    {
        private readonly AppDbContext _dbContext;

        public ProblemService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> CreateProblem(CreateProblemRequestModel requestModel)
        {
            var newProblem = new Problem()
            {
                TopicId = requestModel.TopicId,
                AuthorId = requestModel.AuthorId,
                IsDeleted = false,
                IsPublished = requestModel.IsPublished,
                IsPublic = requestModel.IsPublic
            };

            await _dbContext.Problems.AddAsync(newProblem);
            await _dbContext.SaveChangesAsync();

            var newVersion = new ProblemVersion()
            {
                ProblemId = newProblem.Id,
                Type = requestModel.Type,
                Difficulty = requestModel.Difficulty,
                Statement = requestModel.Statement,
                CorrectAnswer = requestModel.correctAnswer,
                Explanation = requestModel.Explanation,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await _dbContext.ProblemVersions.AddAsync(newVersion);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> DeleteProblem(DeleteProblemRequestModel requestModel)
        {
            var problem = await _dbContext.Problems
                .Where(p => p.Id == requestModel.ProblemId)
                .FirstOrDefaultAsync();

            if (problem == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.ProblemNotFound,
                    ErrorMessage = "Problem not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (problem.AuthorId != requestModel.UserId)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            problem.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult<ProblemDetailsModel>> GetProblemDetailsAsync(ProblemDetailsRequestModel requestModel)
        {
            var problemQuery = _dbContext.Problems
                .Where(p =>
                    p.Id == requestModel.ProblemId
                    && !p.IsDeleted)
                .AsNoTracking();

            var problem = await problemQuery
                .Select(p => new
                {
                    p.Id,
                    p.TopicId,
                    p.AuthorId,
                    p.IsDeleted,
                    p.IsPublished,
                    p.IsPublic,
                    ActiveVersion = p.Versions
                        .Where(v => v.IsActive)
                        .Select(v => new
                        {
                            v.Type,
                            v.Difficulty,
                            v.Statement,
                            v.CorrectAnswer,
                            v.Explanation,
                            v.CreatedAt
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (problem == null || problem.ActiveVersion == null)
            {
                return new OperationResult<ProblemDetailsModel>()
                {
                    ErrorCode = ErrorCodeConstants.ProblemNotFound,
                    ErrorMessage = "Problem not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            bool hasAccess = problem.AuthorId == requestModel.UserId
                || problem.IsPublic;

            if (!hasAccess)
            {
                var assignedTestIds = await _dbContext.StudentAssignments
                    .Where(sa => sa.StudentId == requestModel.UserId)
                    .Select(sa => sa.TestId)
                    .Distinct()
                    .ToListAsync();

                var isInAssignedTest = await _dbContext.TestProblems
                    .Where(tp =>
                        tp.ProblemId == requestModel.ProblemId
                        && assignedTestIds.Contains(tp.TestId))
                    .AnyAsync();

                hasAccess = isInAssignedTest;
            }

            if (!hasAccess)
            {
                return new OperationResult<ProblemDetailsModel>()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You do not have access to this problem",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var details = new ProblemDetailsModel()
            {
                ProblemVersionId = problem.Id,
                TopicId = problem.TopicId,
                AuthorId = problem.AuthorId,
                IsDeleted = problem.IsDeleted,
                IsPublished = problem.IsPublished,
                IsPublic = problem.IsPublic,
                Type = problem.ActiveVersion.Type,
                Difficulty = problem.ActiveVersion.Difficulty,
                Statement = problem.ActiveVersion.Statement,
                CorrectAnswer = problem.ActiveVersion.CorrectAnswer,
                Explanation = problem.ActiveVersion.Explanation,
                CreatedAt = problem.ActiveVersion.CreatedAt
            };

            return new OperationResult<ProblemDetailsModel>(details);
        }

        public async Task<OperationResult<PaginatedListResponseModel<ProblemListItemModel>>> GetProblemListAsync(ProblemListRequestModel requestModel)
        {
            var assignedTestIds = await _dbContext.StudentAssignments
                .Where(sa => sa.StudentId == requestModel.UserId)
                .Select(sa => sa.TestId)
                .Distinct()
                .ToListAsync();

            var assignedProblemIds = await _dbContext.TestProblems
                .Where(tp => assignedTestIds.Contains(tp.TestId))
                .Select(tp => tp.ProblemId)
                .Distinct()
                .ToListAsync();

            var problemsQuery = _dbContext.Problems
                .Where(p =>
                    !p.IsDeleted
                    && (p.AuthorId == requestModel.UserId
                        || p.IsPublic
                        || assignedProblemIds.Contains(p.Id)))
                .AsNoTracking();

            if (requestModel.OnlyMyProblems)
            {
                problemsQuery = problemsQuery.Where(p => p.AuthorId == requestModel.UserId);
            }

            if (requestModel.SubjectId > 0)
            {
                problemsQuery = problemsQuery.Where(p => p.Topic.Section.SubjectId == requestModel.SubjectId);
            }

            if (requestModel.TopicId.HasValue)
            {
                problemsQuery = problemsQuery.Where(p => p.TopicId == requestModel.TopicId.Value);
            }

            if (requestModel.Type.HasValue)
            {
                problemsQuery = problemsQuery.Where(p => p.Versions.Any(v => v.IsActive && v.Type == requestModel.Type.Value));
            }

            if (requestModel.Difficulty.HasValue)
            {
                problemsQuery = problemsQuery.Where(p => p.Versions.Any(v => v.IsActive && v.Difficulty == requestModel.Difficulty.Value));
            }

            if (requestModel.IsPublished)
            {
                problemsQuery = problemsQuery.Where(p => p.IsPublished == requestModel.IsPublished);
            }

            if (requestModel.IsPublic)
            {
                problemsQuery = problemsQuery.Where(p => p.IsPublic == requestModel.IsPublic);
            }

            if (!string.IsNullOrWhiteSpace(requestModel.SearchTerm))
            {
                if (requestModel.SearchType == ProblemSearchTypeEnum.ByProblemStatement)
                {
                    problemsQuery = problemsQuery.Where(p =>
                        p.Versions.Any(v =>
                            v.IsActive
                            && v.Statement.StartsWith(requestModel.SearchTerm!)));
                }
                else if (requestModel.SearchType == ProblemSearchTypeEnum.ByAuthorName)
                {
                    problemsQuery = problemsQuery.Where(p =>
                        p.Author.Username.StartsWith(requestModel.SearchTerm!));
                }
            }

            var problemsCount = await problemsQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)problemsCount / requestModel.PageSize);

            var problemPageList = await problemsQuery
                .Select(p => new ProblemListItemModel()
                {
                    ProblemId = p.Id,
                    TopicName = p.Topic.Name,
                    StatementFragment = p.Versions
                        .Where(v => v.IsActive)
                        .Select(v => v.Statement.Length > 100 ? v.Statement.Substring(0, 100) : v.Statement)
                        .FirstOrDefault() ?? string.Empty,
                    ProblemType = p.Versions
                        .Where(v => v.IsActive)
                        .Select(v => v.Type)
                        .FirstOrDefault(),
                    Difficulty = p.Versions
                        .Where(v => v.IsActive)
                        .Select(v => v.Difficulty)
                        .FirstOrDefault()
                })
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .ToListAsync();

            var paginatedList = new PaginatedListResponseModel<ProblemListItemModel>()
            {
                Items = problemPageList,
                TotalPagesCount = pagesCount,
                Page = requestModel.PageNumber,
                PageSize = requestModel.PageSize,
                HasPreviousPage = requestModel.PageNumber > 1,
                HasNextPage = requestModel.PageNumber < pagesCount
            };

            return new OperationResult<PaginatedListResponseModel<ProblemListItemModel>>(paginatedList);
        }

        public async Task<OperationResult<Problem>> UpdateProblem(UpdateProblemRequestModel requestModel)
        {
            var problem = await _dbContext.Problems
                .Where(p => p.Id == requestModel.AuthorId)
                .FirstOrDefaultAsync();

            if (problem == null)
            {
                return new OperationResult<Problem>()
                {
                    ErrorCode = ErrorCodeConstants.ProblemNotFound,
                    ErrorMessage = "Problem not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (problem.AuthorId != requestModel.AuthorId)
            {
                return new OperationResult<Problem>()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            problem.TopicId = requestModel.TopicId;
            problem.IsPublished = requestModel.IsPublished;
            problem.IsPublic = requestModel.IsPublic;

            var activeVersion = await _dbContext.ProblemVersions
                .Where(v =>
                    v.ProblemId == requestModel.AuthorId
                    && v.IsActive)
                .FirstOrDefaultAsync();

            if (activeVersion != null)
            {
                activeVersion.IsActive = false;
            }

            var newVersion = new ProblemVersion()
            {
                ProblemId = problem.Id,
                Type = activeVersion?.Type ?? Domain.Common.Enums.ProblemType.SingleChoice,
                Difficulty = requestModel.Difficulty,
                Statement = requestModel.Statement,
                CorrectAnswer = requestModel.correctAnswer,
                Explanation = requestModel.Explanation,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await _dbContext.ProblemVersions.AddAsync(newVersion);
            await _dbContext.SaveChangesAsync();

            return new OperationResult<Problem>(problem);
        }
    }
}
