namespace CTHelper.Application.Models.Group
{
    public class AddStudentToGroupModel
    {
        public long TeacherId { get; set; }
        public long StudentId { get; set; }
        public long GroupId { get; set; }
    }
}
