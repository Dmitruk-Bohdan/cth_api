using CTHelper.Application.Common.Enums;
using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Problem
{
    public class ProblemListRequestModel
    {
        public long SubjectId { get; set; }
        public ProblemSearchTypeEnum SearchType { get; set; }
        public long UserId { get; set; }
        public long? TopicId { get; set; }
        public ProblemTypeEnum? Type { get; set; }
        public ProblemDifficultEnum? Difficulty { get; set; }
        public string? SearchTerm { get; set; }
        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }
        public bool OnlyMyProblems { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
