using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Test
{
    public class TestPreviewModel
    {
        public long TestId { get; set; }
        public string TestName { get; set; } = default!;
        public long AuthorId { get; set; }
        public string AuthorName { get; set; } = default!;
        public int ProblemCount { get; set; }
        public ProblemDifficult AvgDifficult { get; set; }
    }
}
