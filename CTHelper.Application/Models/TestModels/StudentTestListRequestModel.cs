using CTHelper.Domain.Common.Enums;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TestModels
{
    public class StudentTestListRequestModel : BaseTestListRequestModel
    {
        public bool? AssignedToMe { get; set; }
        public long UserId { get; set; }
    }
}
