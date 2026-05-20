using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Models.TestAttemptModels
{
    public class TestAttemptDetails
    {
        public long TestAttemptId { get; set; }
        public string TestName { get; set; } = default!;
        public long TestId { get; set; }
        public long StudentId { get; set; }
        public string StudentName { get; set; } = default!;
        public TestAttemptStatusTypeEnum Status { get; set; }
        public int Duration { get; set; }
        public short? RawScore { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public IEnumerable<UserAnswerModel> UserAnswers { get; set; } = new List<UserAnswerModel>();
    }

    public class UserAnswerModel
    {
        public long ProblemId { get; set; }
        public bool IsActualProblemVersion { get; set; }
        public string Statement { get; set; } = default!;
        public string Answer { get; set; } = default!;
        public bool IsCorrect { get; set; }
        public string CorrectAnswer { get; set; } = default!;
        public string? Explanation { get; set; } = default!;
        public string ProblemCode { get; set; } = default!;
        public ProblemTypeEnum Type { get; set; }
        public ProblemDifficultEnum Difficulty { get; set; }
        public string TopicName { get; set; } = default!;
    }
}
