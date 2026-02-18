namespace СTHelper.Domain.Entities
{
    public class TestProblem : BaseEntity
    {
        public long TestId { get; set; }

        public long ProblemId { get; set; }

        public string Code { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }

        public Test Test { get; set; } = default!;

        public Problem Problem { get; set; } = default!;
    }
}


