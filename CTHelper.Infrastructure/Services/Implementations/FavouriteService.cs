using CTHelper.Application.Common.Constants;
using CTHelper.Domain.Common.Enums;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Favourite;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Models.TestModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class FavouriteService : IFavouriteService
    {
        private readonly AppDbContext _dbContext;

        public FavouriteService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> AddProblemToFavourite(AddProblemToFavouriteRequestModel requestModel)
        {
            var problemExists = await _dbContext.Problems
                .Where(p => !p.IsDeleted && p.Id == requestModel.ProblemId)
                .AnyAsync();

            if (!problemExists)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.ProblemNotFound,
                    ErrorMessage = "Specified problem is not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var alreadyInFavourites = await _dbContext.Users
                .Where(u => u.Id == requestModel.UserId)
                .AnyAsync(u => u.FavoriteProblems.Any(p => p.Id == requestModel.ProblemId));

            if (alreadyInFavourites)
            {
                return new OperationResult();
            }

            var userStub = new User { Id = requestModel.UserId };
            var problemStub = new Problem { Id = requestModel.ProblemId };

            _dbContext.Users.Attach(userStub);
            _dbContext.Problems.Attach(problemStub);

            userStub.FavoriteProblems.Add(problemStub);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> AddTestToFavourite(AddTestToFavouriteRequestModel requestModel)
        {
            var testExists = await _dbContext.Tests
                .Where(t => !t.IsDeleted && t.Id == requestModel.TestId)
                .AnyAsync();

            if (!testExists)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Specified test is not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var alreadyInFavourites = await _dbContext.Users
                .Where(u => u.Id == requestModel.UserId)
                .AnyAsync(u => u.FavoriteTests.Any(t => t.Id == requestModel.TestId));

            if (alreadyInFavourites)
            {
                return new OperationResult();
            }

            var userStub = new User { Id = requestModel.UserId };
            var testStub = new Test { Id = requestModel.TestId };

            _dbContext.Users.Attach(userStub);
            _dbContext.Tests.Attach(testStub);

            userStub.FavoriteTests.Add(testStub);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult<PaginatedListResponseModel<ProblemListItemModel>>> GetMyFavouriteProblemList(MyFavouriteProblemListRequestModel requestModel)
        {
            var countQuery = _dbContext.Users
                .Where(u => u.Id == requestModel.UserId)
                .SelectMany(u => u.FavoriteProblems)
                .Where(p => !p.IsDeleted)
                .AsNoTracking();

            var problemsCount = await countQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)problemsCount / requestModel.PageSize);

            var problemPageList = await countQuery
                .Select(p => new ProblemListItemModel()
                {
                    ProblemId = p.Id,
                    TopicName = p.Topic.Name,
                    StatementFragment = p.Versions
                        .FirstOrDefault(v => v.IsActive)!
                        .Statement,
                    ProblemType = p.Versions
                        .FirstOrDefault(v => v.IsActive)!
                        .Type,
                    Difficulty = p.Versions
                        .FirstOrDefault(v => v.IsActive)!
                        .Difficulty
                })
                .OrderByDescending(p => p.ProblemId)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .ToListAsync();

            var paginatedProblemList = new PaginatedListResponseModel<ProblemListItemModel>()
            {
                Items = problemPageList,
                TotalPagesCount = pagesCount,
                Page = requestModel.PageNumber,
                PageSize = requestModel.PageSize,
                HasPreviousPage = requestModel.PageNumber > 1,
                HasNextPage = requestModel.PageNumber < pagesCount
            };

            var result = new OperationResult<PaginatedListResponseModel<ProblemListItemModel>>(paginatedProblemList);
            return result;
        }

        public async Task<OperationResult<PaginatedListResponseModel<TestPreviewModel>>> GetMyFavouriteTestList(MyFavouriteTestListRequestModel requestModel)
        {
            var countQuery = _dbContext.Users
                .Where(u => u.Id == requestModel.UserId)
                .SelectMany(u => u.FavoriteTests)
                .Where(t => !t.IsDeleted)
                .AsNoTracking();

            var testsCount = await countQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)testsCount / requestModel.PageSize);

            var testPageListRaw = await countQuery
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.AuthorId,
                    AuthorName = t.Author.Username,
                    ProblemCount = t.TestProblems.Count,
                    t.Type,
                    AttemptsLeft = t.AttemptsCount,
                    AvgDifficultRaw = t.TestProblems
                        .Average(tp => tp.Problem.Versions
                            .Where(v => v.IsActive)
                            .Select(v => (int?)v.Difficulty)
                            .FirstOrDefault())
                })
                .OrderByDescending(x => x.Id)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .ToListAsync();

            var testPageList = testPageListRaw.Select(t => new TestPreviewModel
            {
                TestId = t.Id,
                TestName = t.Title,
                AuthorId = t.AuthorId,
                AuthorName = t.AuthorName,
                ProblemCount = t.ProblemCount,
                IsAssigned = false,
                Type = t.Type,
                AttemptsLeft = t.AttemptsLeft,
                AvgDifficult = (ProblemDifficultEnum)(t.AvgDifficultRaw.HasValue
                    ? Math.Round(t.AvgDifficultRaw.Value)
                    : 0)
            }).ToList();

            var paginatedTestList = new PaginatedListResponseModel<TestPreviewModel>()
            {
                Items = testPageList,
                TotalPagesCount = pagesCount,
                Page = requestModel.PageNumber,
                PageSize = requestModel.PageSize,
                HasPreviousPage = requestModel.PageNumber > 1,
                HasNextPage = requestModel.PageNumber < pagesCount
            };

            var result = new OperationResult<PaginatedListResponseModel<TestPreviewModel>>(paginatedTestList);
            return result;
        }

        public async Task<OperationResult> RemoveProblemFromFavourite(RemoveProblemFromFavouriteRequestModel requestModel)
        {
            var inFavourites = await _dbContext.Users
                .Where(u => u.Id == requestModel.UserId)
                .AnyAsync(u => u.FavoriteProblems.Any(p => p.Id == requestModel.ProblemId));

            if (!inFavourites)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.ProblemNotInFavourites,
                    ErrorMessage = "Specified problem is not in your favourites",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var userStub = new User { Id = requestModel.UserId };
            var problemStub = new Problem { Id = requestModel.ProblemId };

            _dbContext.Users.Attach(userStub);
            _dbContext.Problems.Attach(problemStub);

            userStub.FavoriteProblems.Remove(problemStub);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> RemoveTestFromFromFavourite(RemoveTestFromFavouriteRequestModel requestModel)
        {
            var inFavourites = await _dbContext.Users
                .Where(u => u.Id == requestModel.UserId)
                .AnyAsync(u => u.FavoriteTests.Any(t => t.Id == requestModel.TestId));

            if (!inFavourites)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.TestNotInFavourites,
                    ErrorMessage = "Specified test is not in your favourites",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var userStub = new User { Id = requestModel.UserId };
            var testStub = new Test { Id = requestModel.TestId };

            _dbContext.Users.Attach(userStub);
            _dbContext.Tests.Attach(testStub);

            userStub.FavoriteTests.Remove(testStub);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }
    }
}