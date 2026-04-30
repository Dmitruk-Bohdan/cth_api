using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.TestModels
{
    public class TestDetailsModel : TestPreviewModel
    {
        public IEnumerable<TestProblemModel> Problems { get; set; } = new List<TestProblemModel>();
    }

    public class TestProblemModel
    {
        public long ProblemId { get; set; }
        public string Code { get; set; } = default!;
        public ProblemTypeEnum Type { get; set; }
        public ProblemDifficultEnum Difficulty { get; set; }
        public string Statement { get; set; } = default!;
    }
}
