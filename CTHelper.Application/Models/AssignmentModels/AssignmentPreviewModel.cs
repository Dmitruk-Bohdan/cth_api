namespace CTHelper.Application.Models.Assignment
{
    public class AssignmentPreviewModel
    {
        public long AssignmentId { get; set; }
        public long TeacherName { get; set; }
        public long TeacherId { get; set; }
        public long TestName { get; set; }
        public DateTimeOffset ExpiredAt { get; set; }
    }
}
