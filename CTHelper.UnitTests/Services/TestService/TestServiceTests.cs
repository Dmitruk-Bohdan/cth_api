using CTHelper.Application.Models.TestModels;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Persistence.Context;
using CTHelper.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CTHelper.UnitTests.Services.TestService;

public class TestServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateMixedTest_Success()
    {
        var ctx = CreateContext();
        ctx.Subjects.Add(new Subject { Id = 1, Name = "Math" });
        ctx.Topics.Add(TestEntityFactory.CreateTopic(1, "Algebra", 1));
        ctx.Problems.Add(TestEntityFactory.CreateProblem(1, 1));
        ctx.ProblemVersions.Add(TestEntityFactory.CreateProblemVersion(1, 1));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestService(ctx);
        var r = await svc.CreateMixedTest(new CreateMixedTestRequestModel { SubjectId = 1, AverageDifficult = ProblemDifficultEnum.Normal, AuthorId = 10, TopicItems = new List<MixedTestTopicModel> { new() { TopicId = 1, ProblemCount = 1 } } });
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public async Task CreateMixedTest_NoProblems()
    {
        var ctx = CreateContext();
        ctx.Subjects.Add(new Subject { Id = 1, Name = "Math" });
        ctx.Topics.Add(TestEntityFactory.CreateTopic(1, "Algebra", 1));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestService(ctx);
        var r = await svc.CreateMixedTest(new CreateMixedTestRequestModel { SubjectId = 1, AverageDifficult = ProblemDifficultEnum.Hard, AuthorId = 10, TopicItems = new List<MixedTestTopicModel> { new() { TopicId = 1, ProblemCount = 5 } } });
        Assert.False(r.IsSuccess);
    }

    [Fact]
    public async Task CreateMixedTest_WithABGrouping()
    {
        var ctx = CreateContext();
        ctx.Subjects.Add(new Subject { Id = 1, Name = "Math" });
        ctx.Topics.Add(TestEntityFactory.CreateTopic(1, "Algebra", 1));
        ctx.Problems.Add(TestEntityFactory.CreateProblem(1, 1));
        ctx.Problems.Add(TestEntityFactory.CreateProblem(2, 1));
        ctx.ProblemVersions.Add(TestEntityFactory.CreateProblemVersion(1, 1, ProblemTypeEnum.SingleChoice));
        ctx.ProblemVersions.Add(TestEntityFactory.CreateProblemVersion(2, 2, ProblemTypeEnum.OpenEnded));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestService(ctx);
        var r = await svc.CreateMixedTest(new CreateMixedTestRequestModel { SubjectId = 1, AverageDifficult = ProblemDifficultEnum.Normal, AuthorId = 10, TopicItems = new List<MixedTestTopicModel> { new() { TopicId = 1, ProblemCount = 2 } } });
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public async Task CreateTest_Success()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestService(ctx);
        var r = await svc.CreateTest(new CreateTestRequestModel { Title = "My Test", SubjectId = 1, AuthorId = 10, IsTraning = true, IsPublished = true, IsPublic = true, Duration = 1800, AttemptsCount = 3, TestProblemList = new List<TestProblemCodeModel>() });
        Assert.True(r.IsSuccess);
        Assert.Single(ctx.Tests);
    }

    [Fact]
    public async Task GetTestDetails_NotFound()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestService(ctx);
        var r = await svc.GetTestDetails(new TestDetailsRequestModel { TestId = 999, UserId = 10 });
        Assert.False(r.IsSuccess);
    }

    [Fact]
    public async Task UpdateTest_Success()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(TestEntityFactory.CreateTest(1, "Old", 10));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestService(ctx);
        var r = await svc.UpdateTest(new UpdateTestRequestModel { TestId = 1, UserId = 10, Title = "New", IsTraning = false, IsPublished = false, IsPublic = false, Duration = 3600, AttemptsCount = 2, TestProblemIdList = new List<TestProblemCodeModel>() });
        Assert.True(r.IsSuccess);
        var t = await ctx.Tests.FirstAsync();
        Assert.Equal("New", t.Title);
    }

    [Fact]
    public async Task UpdateTest_NotFound()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestService(ctx);
        var r = await svc.UpdateTest(new UpdateTestRequestModel { TestId = 999, UserId = 10, Title = "DoesntMatter" });
        Assert.False(r.IsSuccess);
    }

    [Fact]
    public async Task RemoveTest_Success()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(TestEntityFactory.CreateTest(1, "T1", 10));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestService(ctx);
        var r = await svc.RemoveTest(new RemoveTestRequestModel { TestId = 1, UserId = 10 });
        Assert.True(r.IsSuccess);
        var t = await ctx.Tests.FirstAsync();
        Assert.True(t.IsDeleted);
    }

    [Fact]
    public async Task RemoveTest_NotFound()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestService(ctx);
        var r = await svc.RemoveTest(new RemoveTestRequestModel { TestId = 999, UserId = 10 });
        Assert.False(r.IsSuccess);
    }
}