namespace CTHelper.Application.Models.Assignment
{
    public class BaseAssignmentDetailsModel
    {
        public long AssignmentId { get; set; }
        public long TeacherId { get; set; }
        public long TeacherName { get; set; }
        public long TestId { get; set; }
        public long TestName { get; set; }
        public DateTimeOffset ExpiredAt { get; set; }
        public short AttemptsLeft { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
