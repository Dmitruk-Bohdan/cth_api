using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Problem
{
    public class UpdateProblemRequestModel
    {
        public long AuthorId { get; set; }
        public ProblemDifficult Difficulty { get; set; }
        public string Statement { get; set; } = default!;
        public string correctAnswer { get; set; } = default!;
        public string Explanation { get; set; } = default!;
        public long TopicId { get; set; }
        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }
    }
}
