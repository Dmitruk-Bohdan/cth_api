using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TeacherStudent
{
    public class MyBlockedStudentListRequestModel : PaginatedListRequestModel
    {
        public long TeacherId { get; set; }
    }
}