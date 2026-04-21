using CTHelper.Domain.Common.Enums;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TestModels
{
    public class TeacherTestListRequestModel : BaseTestListRequestModel
    {
        public bool? OnlyMyTests { get; set; }
        public long UserId { get; set; }
    }
}
