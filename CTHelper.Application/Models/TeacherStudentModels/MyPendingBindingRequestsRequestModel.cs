using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TeacherStudent
{
    public class MyPendingBindingRequestsRequestModel : PaginatedListRequestModel
    {
        public long TeacherId { get; set; }
    }
}