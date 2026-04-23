namespace CTHelper.Application.Models.Assignment
{
    public class BaseAssignmentDetailsModel
    {
        public long AssignmentId { get; set; }
        public long TeacherId { get; set; }
        public string TeacherName { get; set; } = default!;
        public long TestId { get; set; }
        public string TestName { get; set; } = default!;
        public DateTimeOffset ExpiredAt { get; set; }
        public short AttemptsLeft { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
