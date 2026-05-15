using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.AssignmentModels
{
    public class GetIAssignedRequestModel : PaginatedListRequestModel
    {
        public long UserId { get; set; }
    }
}
