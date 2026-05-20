using CTHelper.Application.Models.TestAttemptModels;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Persistence.Context;
using CTHelper.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CTHelper.UnitTests.Services.TestAttemptService;

public class TestAttemptServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task StartTestAttempt_Success()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(TestEntityFactory.CreateTest(1, "T1", isPublic: true, isPublished: true));
        ctx.Problems.Add(TestEntityFactory.CreateProblem(1, 1));
        ctx.ProblemVersions.Add(TestEntityFactory.CreateProblemVersion(1, 1));
        ctx.TestProblems.Add(new TestProblem { TestId = 1, ProblemId = 1, Code = "A1" });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.StartTestAttempt(new StartTestAttemptRequestModel { TestId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task StartTestAttempt_TestNotFound()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.StartTestAttempt(new StartTestAttemptRequestModel { TestId = 999, UserId = 10 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task StartTestAttempt_NoAccess()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(TestEntityFactory.CreateTest(1, "Private", isPublic: false, isPublished: true));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.StartTestAttempt(new StartTestAttemptRequestModel { TestId = 1, UserId = 10 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task StartTestAttempt_AlreadyActive()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(TestEntityFactory.CreateTest(1, "T1", isPublic: true, isPublished: true));
        ctx.TestAttempts.Add(new TestAttempt { Id = 1, TestId = 1, StudentId = 10, Status = TestAttemptStatusTypeEnum.InProgress, CreatedAt = DateTimeOffset.UtcNow, LastResumedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.StartTestAttempt(new StartTestAttemptRequestModel { TestId = 1, UserId = 10 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task CompleteTestAttempt_Success()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(new Test { Id = 1, Title = "T1", IsTraning = true, IsPublished = true, SubjectId = 0, AuthorId = 0, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow, Type = TestTypeEnum.Custom });
        ctx.Problems.Add(TestEntityFactory.CreateProblem(1, 1));
        ctx.ProblemVersions.Add(TestEntityFactory.CreateProblemVersion(1, 1, ans: "right"));
        ctx.TestProblems.Add(new TestProblem { TestId = 1, ProblemId = 1, Code = "A1" });
        ctx.TestAttempts.Add(new TestAttempt { Id = 1, TestId = 1, StudentId = 10, Status = TestAttemptStatusTypeEnum.InProgress, CreatedAt = DateTimeOffset.UtcNow, LastResumedAt = DateTimeOffset.UtcNow.AddMinutes(-5) });
        ctx.UserAnswers.Add(new UserAnswer { Id = 1, TestAttemptId = 1, ProblemVersionId = 1, Answer = "right", IsCorrect = false, CreatedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.CompleteTestAttempt(new CompleteTestAttemptRequestModel { AttemptId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CompleteTestAttempt_NotFound()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.CompleteTestAttempt(new CompleteTestAttemptRequestModel { AttemptId = 999, UserId = 10 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task PauseTestAttempt_Success()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(new Test { Id = 1, Title = "T1", IsTraning = true, IsPublished = true, SubjectId = 0, AuthorId = 0, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow, Type = TestTypeEnum.Custom });
        ctx.TestAttempts.Add(new TestAttempt { Id = 1, TestId = 1, StudentId = 10, Status = TestAttemptStatusTypeEnum.InProgress, CreatedAt = DateTimeOffset.UtcNow, LastResumedAt = DateTimeOffset.UtcNow.AddMinutes(-3) });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.PauseTestAttempt(new PauseTestAttemptRequestModel { AttemptId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task PauseTestAttempt_NotTraining()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(new Test { Id = 1, Title = "Exam", IsTraning = false, IsPublished = true, SubjectId = 0, AuthorId = 0, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow, Type = TestTypeEnum.Custom });
        ctx.TestAttempts.Add(new TestAttempt { Id = 1, TestId = 1, StudentId = 10, Status = TestAttemptStatusTypeEnum.InProgress, CreatedAt = DateTimeOffset.UtcNow, LastResumedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.PauseTestAttempt(new PauseTestAttemptRequestModel { AttemptId = 1, UserId = 10 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ResumeTestAttempt_Success()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(new Test { Id = 1, Title = "T1", IsTraning = true, IsPublished = true, SubjectId = 0, AuthorId = 0, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow, Type = TestTypeEnum.Custom });
        ctx.TestAttempts.Add(new TestAttempt { Id = 1, TestId = 1, StudentId = 10, Status = TestAttemptStatusTypeEnum.Paused, CreatedAt = DateTimeOffset.UtcNow, LastResumedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.ResumeTestAttempt(new ResumeTestAttemptRequestModel { AttemptId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetMyAttempt_Success()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(TestEntityFactory.CreateTest(1, "T1", isPublic: true));
        ctx.TestAttempts.Add(new TestAttempt { Id = 1, TestId = 1, StudentId = 10, Status = TestAttemptStatusTypeEnum.Completed, CreatedAt = DateTimeOffset.UtcNow, LastResumedAt = DateTimeOffset.UtcNow });
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "stud"));
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.TestAttemptService(ctx);
        var result = await svc.GetMyAttempt(new MyTestAttemptRequestModel { AttemptId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
    }
}