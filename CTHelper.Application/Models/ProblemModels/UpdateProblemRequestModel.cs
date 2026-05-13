using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Problem
{
    public class UpdateProblemRequestModel
    {
        public long ProblemId { get; set; }
        public long AuthorId { get; set; }
        public ProblemDifficultEnum Difficulty { get; set; }
        public string Statement { get; set; } = default!;
        public string correctAnswer { get; set; } = default!;
        public string Explanation { get; set; } = default!;
        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }
    }
}
