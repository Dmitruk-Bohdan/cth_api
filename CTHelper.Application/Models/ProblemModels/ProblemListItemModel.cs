using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Problem
{
    public class ProblemListItemModel
    {
        public long ProblemId { get; set; }
        public string TopicName { get; set; } = default!;
        public string StatementFragment { get; set; } = default!;
        public ProblemTypeEnum ProblemType { get; set; }
        public ProblemDifficultEnum Difficulty { get; set; }

    }
}
