using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.TestModels
{
    public class TestListItemModel
    {
        public long TestId { get; set; }
        public string TestName { get; set; } = default!;
        public string AuthorName { get; set; } = default!;
        public int ProblemCount { get; set; }
        public bool IsPublished { get; set; }
        public ProblemDifficultEnum AvgDifficult { get; set; }
    }
}
