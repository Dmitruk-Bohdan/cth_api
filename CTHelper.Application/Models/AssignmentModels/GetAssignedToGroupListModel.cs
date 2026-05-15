using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.Assignment
{
    public class GetAssignedToGroupListModel : PaginatedListRequestModel
    {
        public long GroupId { get; set; }
        public long TeacherId { get; set; }
    }
}
