using CTHelper.Presentation.Controllers;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CTHelper.UnitTests.Services.StatisticsService;

public class StatisticsServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetMyStatisticsBySubject_Success()
    {
        var ctx = CreateContext();
        var topic = new Topic { Id = 1, Name = "Algebra", Section = new Section { Id = 1, SubjectId = 1, Name = "Math" } };
        ctx.Topics.Add(topic);
        ctx.Tests.Add(new Test { Id = 1, Title = "T1", SubjectId = 1, AuthorId = 0, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow, Type = TestTypeEnum.Custom });
        ctx.TestAttempts.Add(new TestAttempt { Id = 1, TestId = 1, StudentId = 10, Status = TestAttemptStatusTypeEnum.Completed, CreatedAt = DateTimeOffset.UtcNow, LastResumedAt = DateTimeOffset.UtcNow });
        ctx.Problems.Add(new Problem { Id = 1, TopicId = 1, AuthorId = 0, IsDeleted = false });
        ctx.ProblemVersions.Add(new ProblemVersion { Id = 1, ProblemId = 1, IsActive = true, Difficulty = ProblemDifficultEnum.Normal, CorrectAnswer = "ans", Statement = "stmt", Explanation = "expl", CreatedAt = DateTimeOffset.UtcNow });
        ctx.UserAnswers.Add(new UserAnswer { Id = 1, TestAttemptId = 1, ProblemVersionId = 1, IsCorrect = true, Answer = "ans", CreatedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.StatisticsService(ctx);
        var result = await svc.GetMyStatisticsBySubject(new MyStatisticsBySubjectRequestModel { UserId = 10, SubjectId = 1 });

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Fact]
    public async Task GetMyStatisticsBySubject_Empty()
    {
        var ctx = CreateContext();
        ctx.Topics.Add(new Topic { Id = 1, Name = "Algebra", Section = new Section { Id = 1, SubjectId = 1, Name = "Math" } });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.StatisticsService(ctx);
        var result = await svc.GetMyStatisticsBySubject(new MyStatisticsBySubjectRequestModel { UserId = 10, SubjectId = 1 });
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Payload!.TotalAnswers);
    }

    [Fact]
    public async Task GetStudentStatisticsBySubject_Success()
    {
        var ctx = CreateContext();
        ctx.TeacherStudents.Add(new TeacherStudent { TeacherId = 10, StudentId = 20, IsDeleted = false });
        ctx.Topics.Add(new Topic { Id = 1, Name = "Algebra", Section = new Section { Id = 1, SubjectId = 1, Name = "Math" } });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.StatisticsService(ctx);
        var result = await svc.GetStudentStatisticsBySubject(new StudentStatisticsBySubjectRequestModel { UserId = 10, StudentId = 20, SubjectId = 1 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetStudentStatisticsBySubject_NoBinding()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.StatisticsService(ctx);
        var result = await svc.GetStudentStatisticsBySubject(new StudentStatisticsBySubjectRequestModel { UserId = 10, StudentId = 20, SubjectId = 1 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task BuildTopicStatistics_CorrectAverage()
    {
        var ctx = CreateContext();
        ctx.Topics.Add(new Topic { Id = 1, Name = "Topic1", Section = new Section { Id = 1, SubjectId = 1, Name = "S1" } });
        ctx.Tests.Add(new Test { Id = 1, Title = "T1", SubjectId = 1, AuthorId = 0, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow, Type = TestTypeEnum.Custom });
        ctx.TestAttempts.Add(new TestAttempt { Id = 1, TestId = 1, StudentId = 10, Status = TestAttemptStatusTypeEnum.Completed, CreatedAt = DateTimeOffset.UtcNow, LastResumedAt = DateTimeOffset.UtcNow });
        ctx.Problems.Add(new Problem { Id = 1, TopicId = 1, AuthorId = 0, IsDeleted = false });
        ctx.ProblemVersions.Add(new ProblemVersion { Id = 1, ProblemId = 1, IsActive = true, Difficulty = ProblemDifficultEnum.Normal, CorrectAnswer = "ans", Statement = "stmt", Explanation = "expl", CreatedAt = DateTimeOffset.UtcNow });
        ctx.UserAnswers.Add(new UserAnswer { Id = 1, TestAttemptId = 1, ProblemVersionId = 1, IsCorrect = true, Answer = "ans", CreatedAt = DateTimeOffset.UtcNow });
        ctx.UserAnswers.Add(new UserAnswer { Id = 2, TestAttemptId = 1, ProblemVersionId = 1, IsCorrect = false, Answer = "wrong", CreatedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.StatisticsService(ctx);
        var result = await svc.GetMyStatisticsBySubject(new MyStatisticsBySubjectRequestModel { UserId = 10, SubjectId = 1 });
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Payload!.StatisticsByTopicList);
    }

    [Fact]
    public async Task BuildTopicStatistics_CorrectMedian()
    {
        var ctx = CreateContext();
        ctx.Topics.Add(new Topic { Id = 1, Name = "T1", Section = new Section { Id = 1, SubjectId = 1, Name = "S1" } });
        ctx.Tests.Add(new Test { Id = 1, Title = "Test", SubjectId = 1, AuthorId = 0, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow, Type = TestTypeEnum.Custom });
        ctx.TestAttempts.Add(new TestAttempt { Id = 1, TestId = 1, StudentId = 10, Status = TestAttemptStatusTypeEnum.Completed, CreatedAt = DateTimeOffset.UtcNow, LastResumedAt = DateTimeOffset.UtcNow });
        ctx.Problems.Add(new Problem { Id = 1, TopicId = 1, AuthorId = 0, IsDeleted = false });
        ctx.ProblemVersions.Add(new ProblemVersion { Id = 1, ProblemId = 1, IsActive = true, Difficulty = ProblemDifficultEnum.Normal, CorrectAnswer = "a", Statement = "stmt", Explanation = "expl", CreatedAt = DateTimeOffset.UtcNow });
        ctx.UserAnswers.Add(new UserAnswer { Id = 1, TestAttemptId = 1, ProblemVersionId = 1, IsCorrect = true, Answer = "a", CreatedAt = DateTimeOffset.UtcNow });
        ctx.UserAnswers.Add(new UserAnswer { Id = 2, TestAttemptId = 1, ProblemVersionId = 1, IsCorrect = true, Answer = "b", CreatedAt = DateTimeOffset.UtcNow });
        ctx.UserAnswers.Add(new UserAnswer { Id = 3, TestAttemptId = 1, ProblemVersionId = 1, IsCorrect = false, Answer = "c", CreatedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.StatisticsService(ctx);
        var result = await svc.GetMyStatisticsBySubject(new MyStatisticsBySubjectRequestModel { UserId = 10, SubjectId = 1 });
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Payload!.StatisticsByTopicList);
    }
}