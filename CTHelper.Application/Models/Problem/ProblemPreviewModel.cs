using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Problem
{
    public class ProblemPreviewModel
    {
        public long ProblemId { get; set; }
        public string TopicName { get; set; } = default!;
        public string StatementFragment { get; set; } = default!;
        public ProblemType ProblemType { get; set; }
        public ProblemDifficult Difficulty { get; set; }

    }
}
