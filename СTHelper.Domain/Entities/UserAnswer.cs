namespace СTHelper.Domain.Entities
{
    public class UserAnswer : BaseEntity
    {
        public long TestAttemptId { get; set; }

        public long ProblemVersionId { get; set; }

        public string Answer { get; set; } = default!;

        public bool IsCorrect { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public TestAttempt TestAttempt { get; set; } = default!;

        public ProblemVersion ProblemVersion { get; set; } = default!;
    }
}


