using CTHelper.Domain.Common.Enums;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TestModels
{
    public abstract class BaseTestListRequestModel : PaginatedListRequestModel
    {
        public string? NameFragment { get; set; }
        public string? AuthorNameFragment { get; set; }
        public ProblemDifficult? AvgDifficult { get; set; }
        public bool? IsTraning { get; set; }
        public TestType? Type { get; set; }
        public int? MaxTaskCount { get; set; }
        public int? MinTaskCount { get; set; }
    }
}
