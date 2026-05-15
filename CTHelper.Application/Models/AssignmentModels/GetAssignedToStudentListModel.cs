using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.Assignment
{
    public class GetAssignedToStudentListModel : PaginatedListRequestModel
    {
        public long StudentId { get; set; }
        public long TeacherId { get; set; }
    }
}
