using CTHelper.Domain.Common.Enums;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TestModels
{
    public class MyTestListRequestModel : PaginatedListRequestModel
    {
        public long UserId { get; set; }
        public string? NameFragment { get; set; }
        public ProblemDifficultEnum? AvgDifficult { get; set; }
        public bool? IsTraning { get; set; }
        public TestTypeEnum? Type { get; set; }
        public int? MaxTaskCount { get; set; }
        public int? MinTaskCount { get; set; }
    }
}
