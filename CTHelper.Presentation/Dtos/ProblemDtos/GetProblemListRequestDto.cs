using CTHelper.Application.Common.Enums;
using CTHelper.Domain.Common.Enums;

namespace CTHelper.Presentation.Dtos.ProblemDtos
{
    public class ProblemListRequestDto
    {
        public long SubjectId { get; set; }
        public ProblemSearchTypeEnum SearchType { get; set; }
        public long? TopicId { get; set; }
        public ProblemType? Type { get; set; }
        public ProblemDifficult? Difficulty { get; set; }
        public string? SearchTerm { get; set; }
        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }
        public bool OnlyMyProblems { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
