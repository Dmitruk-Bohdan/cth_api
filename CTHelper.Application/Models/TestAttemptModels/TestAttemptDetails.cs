using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Models.TestAttemptModels
{
    public class TestAttemptDetails
    {
        public long TestAttemptId { get; set; }
        public long TestName { get; set; }
        public long TestId { get; set; }
        public long StudentId { get; set; }
        public long StudentName { get; set; }
        public TestAttemptStatusType Status { get; set; }
        public int Duration { get; set; }
        public short? RawScore { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public IEnumerable<UserAnswerModel> UserAnswers { get; set; } = new List<UserAnswerModel>();
    }

    public class UserAnswerModel
    {
        public long ProblemId { get; set; }
        public long IsActualProblemVersion { get; set; }
        public string Statement { get; set; } = default!;
        public string Answer { get; set; } = default!;
        public bool IsCorrect { get; set; }
        public string CorrectAnswer { get; set; } = default!;
        public string? Explanation { get; set; } = default!;
        public ProblemType Type { get; set; }
        public ProblemDifficult Difficulty { get; set; }
        public long TopicName { get; set; }
    }
}
