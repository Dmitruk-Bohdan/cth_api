using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;

namespace CTHelper.UnitTests.Helpers;

public static class TestEntityFactory
{
    public static User CreateUser(long id, string name)
        => new()
        {
            Id = id, Username = name, Email = $"{name}@test.com",
            PasswordHash = "hashed_password", CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow
        };

    public static Test CreateTest(long id, string title, long authorId = 0, bool isPublic = true, bool isPublished = true)
        => new()
        {
            Id = id, Title = title, AuthorId = authorId, IsPublic = isPublic, IsPublished = isPublished,
            IsDeleted = false, SubjectId = 0, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow,
            Type = TestTypeEnum.Custom
        };

    public static Problem CreateProblem(long id, long topicId, bool isPublic = true)
        => new() { Id = id, TopicId = topicId, AuthorId = 0, IsDeleted = false, IsPublished = true, IsPublic = isPublic };

    public static ProblemVersion CreateProblemVersion(long id, long problemId, ProblemTypeEnum type = ProblemTypeEnum.SingleChoice, string stmt = "Q", string ans = "A")
        => new()
        {
            Id = id, ProblemId = problemId, IsActive = true, Type = type,
            Difficulty = ProblemDifficultEnum.Normal, Statement = stmt, CorrectAnswer = ans,
            Explanation = "explanation", CreatedAt = DateTimeOffset.UtcNow
        };

    public static Topic CreateTopic(long id, string name, long subjectId)
    {
        var section = new Section { Id = 100 + id, SubjectId = subjectId, Name = $"Section {id}" };
        return new Topic { Id = id, Name = name, SectionId = section.Id, Section = section, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow };
    }
}