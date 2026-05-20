using CTHelper.Application.Models.Favourite;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Persistence.Context;
using CTHelper.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CTHelper.UnitTests.Services.FavouriteService;

public class FavouriteServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddProblemToFavourite_Success()
    {
        var ctx = CreateContext();
        ctx.Problems.Add(new Problem { Id = 1, IsPublic = true, IsDeleted = false });
        ctx.ProblemVersions.Add(new ProblemVersion { Id = 1, ProblemId = 1, IsActive = true, CorrectAnswer = "A", Statement = "Q", Explanation = "expl", CreatedAt = DateTimeOffset.UtcNow });
        var user = TestEntityFactory.CreateUser(10, "user");
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear(); // detach all entities to avoid Attach conflicts in service

        var svc = new CTHelper.Infrastructure.Services.Implementations.FavouriteService(ctx);
        var result = await svc.AddProblemToFavourite(new AddProblemToFavouriteRequestModel { ProblemId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RemoveProblemFromFavourite_Success()
    {
        var ctx = CreateContext();
        var problem = new Problem { Id = 1, IsPublic = true, IsDeleted = false };
        var user = TestEntityFactory.CreateUser(10, "user");
        user.FavoriteProblems.Add(problem);
        ctx.Users.Add(user);
        ctx.Problems.Add(problem);
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.FavouriteService(ctx);
        var result = await svc.RemoveProblemFromFavourite(new RemoveProblemFromFavouriteRequestModel { ProblemId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetMyFavouriteProblemList_Success()
    {
        var ctx = CreateContext();
        var problem = new Problem { Id = 1, IsPublic = true, IsDeleted = false, Topic = new Topic { Id = 1, Name = "Algebra", SectionId = 1, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow },
            Versions = new List<ProblemVersion> { new() { Id = 1, IsActive = true, Statement = "stmt", CorrectAnswer = "A", Explanation = "expl", CreatedAt = DateTimeOffset.UtcNow, Type = Domain.Common.Enums.ProblemTypeEnum.SingleChoice, Difficulty = Domain.Common.Enums.ProblemDifficultEnum.Normal } } };
        var user = TestEntityFactory.CreateUser(10, "user");
        user.FavoriteProblems.Add(problem);
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.FavouriteService(ctx);
        var result = await svc.GetMyFavouriteProblemList(new MyFavouriteProblemListRequestModel { UserId = 10, PageNumber = 1, PageSize = 10 });
        Assert.True(result.IsSuccess);
        Assert.Single(result.Payload!.Items);
    }

    [Fact]
    public async Task RemoveProblemFromFavourite_NotInFavourites()
    {
        var ctx = CreateContext();
        var user = TestEntityFactory.CreateUser(10, "user");
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.FavouriteService(ctx);
        var result = await svc.RemoveProblemFromFavourite(new RemoveProblemFromFavouriteRequestModel { ProblemId = 999, UserId = 10 });
        Assert.False(result.IsSuccess);
    }
}