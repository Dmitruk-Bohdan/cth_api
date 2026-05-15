using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TeacherStudent
{
    public class MyTeachersListRequestModel : PaginatedListRequestModel
    {
        public long StudentId { get; set; }
    }
}