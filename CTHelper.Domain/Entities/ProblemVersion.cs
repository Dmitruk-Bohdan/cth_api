using CTHelper.Domain.Common.Enums;

namespace CTHelper.Domain.Entities
{
    public class ProblemVersion : BaseEntity
    {
        public long ProblemId { get; set; }

        public ProblemTypeEnum Type { get; set; }

        public ProblemDifficultEnum Difficulty { get; set; }

        public string Statement { get; set; } = default!;

        public string CorrectAnswer { get; set; } = default!;

        public string Explanation { get; set; } = default!;

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Problem Problem { get; set; } = default!;
    }
}


