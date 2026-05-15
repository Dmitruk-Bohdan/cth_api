using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TeacherStudent
{
    public class MyStudentsListRequestModel : PaginatedListRequestModel
    {
        public long TeacherId { get; set; }
    }
}