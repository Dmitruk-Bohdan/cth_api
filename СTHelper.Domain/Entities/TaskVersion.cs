using СTHelper.Domain.Common.Enums;

namespace СTHelper.Domain.Entities
{
    public class ProblemVersion : BaseEntity
    {
        public long ProblemId { get; set; }

        public long AuthorId { get; set; }

        public ProblemType Type { get; set; }

        public ProblemDifficulty Difficulty { get; set; }

        public string Statement { get; set; } = default!;

        public string CorrectAnswer { get; set; } = default!;

        public string Explanation { get; set; } = default!;

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Problem Problem { get; set; } = default!;

        public User Author { get; set; } = default!;

        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}


