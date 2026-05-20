using CTHelper.Application.Models.Problem;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Persistence.Context;
using CTHelper.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CTHelper.UnitTests.Services.ProblemService;

public class ProblemServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private static ProblemVersion MakePv(long id, long problemId, string expl = "expl")
        => new() { Id = id, ProblemId = problemId, IsActive = true, Type = ProblemTypeEnum.SingleChoice,
            Difficulty = ProblemDifficultEnum.Normal, Statement = "Q", CorrectAnswer = "A",
            Explanation = expl, CreatedAt = DateTimeOffset.UtcNow };

    [Fact]
    public async Task CreateProblem_Success()
    {
        var ctx = CreateContext();
        ctx.Topics.Add(TestEntityFactory.CreateTopic(1, "Algebra", 1));
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "author"));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.ProblemService(ctx);
        var result = await svc.CreateProblem(new CreateProblemRequestModel
        {
            TopicId = 1, AuthorId = 10, Type = ProblemTypeEnum.SingleChoice,
            Difficulty = ProblemDifficultEnum.Normal, Statement = "What is 2+2?",
            correctAnswer = "4", Explanation = "Basic addition", IsPublished = true, IsPublic = true
        });
        Assert.True(result.IsSuccess);
        Assert.Single(ctx.Problems);
        Assert.Single(ctx.ProblemVersions);
    }

    [Fact]
    public async Task GetProblemDetailsAsync_Success()
    {
        var ctx = CreateContext();
        ctx.Topics.Add(TestEntityFactory.CreateTopic(1, "Algebra", 1));
        ctx.Problems.Add(new Problem { Id = 1, TopicId = 1, AuthorId = 10, IsPublic = true, IsDeleted = false });
        ctx.ProblemVersions.Add(MakePv(1, 1));
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "author"));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.ProblemService(ctx);
        var result = await svc.GetProblemDetailsAsync(new ProblemDetailsRequestModel { ProblemId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Fact]
    public async Task GetProblemDetailsAsync_NoAccess()
    {
        var ctx = CreateContext();
        ctx.Topics.Add(TestEntityFactory.CreateTopic(1, "Algebra", 1));
        ctx.Problems.Add(new Problem { Id = 1, TopicId = 1, AuthorId = 10, IsPublic = false, IsDeleted = false });
        ctx.ProblemVersions.Add(MakePv(1, 1));
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "author"));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.ProblemService(ctx);
        var result = await svc.GetProblemDetailsAsync(new ProblemDetailsRequestModel { ProblemId = 1, UserId = 20 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateProblem_Success()
    {
        var ctx = CreateContext();
        ctx.Topics.Add(TestEntityFactory.CreateTopic(1, "Algebra", 1));
        ctx.Problems.Add(new Problem { Id = 1, TopicId = 1, AuthorId = 10, IsPublished = false, IsDeleted = false });
        ctx.ProblemVersions.Add(MakePv(1, 1, "old_expl"));
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "author"));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.ProblemService(ctx);
        var result = await svc.UpdateProblem(new UpdateProblemRequestModel
        {
            ProblemId = 1, AuthorId = 10, Difficulty = ProblemDifficultEnum.Hard,
            Statement = "New statement", correctAnswer = "B", Explanation = "New explanation",
            IsPublished = true, IsPublic = true
        });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateProblem_NotFound()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.ProblemService(ctx);
        var result = await svc.UpdateProblem(new UpdateProblemRequestModel { ProblemId = 999, AuthorId = 10, Statement = "X" });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteProblem_Success()
    {
        var ctx = CreateContext();
        ctx.Topics.Add(TestEntityFactory.CreateTopic(1, "Algebra", 1));
        ctx.Problems.Add(new Problem { Id = 1, TopicId = 1, AuthorId = 10, IsDeleted = false });
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "author"));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.ProblemService(ctx);
        var result = await svc.DeleteProblem(new DeleteProblemRequestModel { ProblemId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
        var p = await ctx.Problems.FirstAsync();
        Assert.True(p.IsDeleted);
    }

    [Fact]
    public async Task GetProblemDetailsAsync_NotFound()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.ProblemService(ctx);
        var result = await svc.GetProblemDetailsAsync(new ProblemDetailsRequestModel { ProblemId = 999, UserId = 10 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteProblem_NotFound()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.ProblemService(ctx);
        var result = await svc.DeleteProblem(new DeleteProblemRequestModel { ProblemId = 999, UserId = 10 });
        Assert.False(result.IsSuccess);
    }
}