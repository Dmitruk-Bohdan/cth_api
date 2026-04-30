using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Problem
{
    public class ProblemDetailsModel
    {
        public long ProblemVersionId { get; set; }
        public long TopicId { get; set; }
        public long AuthorId { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }

        public ProblemTypeEnum Type { get; set; }
        public ProblemDifficultEnum Difficulty { get; set; }

        public string Statement { get; set; } = default!;
        public string CorrectAnswer { get; set; } = default!;
        public string Explanation { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
