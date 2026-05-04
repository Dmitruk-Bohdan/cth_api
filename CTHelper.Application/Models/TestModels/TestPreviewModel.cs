using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.TestModels
{
    public class TestPreviewModel
    {
        public long TestId { get; set; }
        public string TestName { get; set; } = default!;
        public long AuthorId { get; set; }
        public string AuthorName { get; set; } = default!;
        public int ProblemCount { get; set; }
        public bool IsAssigned { get; set; }
        public TestTypeEnum Type { get; set; }
        public int? AttemptsLeft { get; set; }
        public ProblemDifficultEnum AvgDifficult { get; set; }
    }
}
