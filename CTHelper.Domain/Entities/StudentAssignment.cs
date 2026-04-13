namespace CTHelper.Domain.Entities
{
    public class StudentAssignment : BaseEntity
    {
        public long StudentId { get; set; }
        public long TeacherId { get; set; }
        public long? GroupAssignmentId { get; set; }
        public long TestId { get; set; }
        public DateTimeOffset ExpiredAt { get; set; }
        public short AttemptsLeft { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdateAt { get; set; }
        public User Teacher { get; set; } = default!;
        public User Student { get; set; } = default!;
        public Test Test { get; set; } = default!;
        public GroupAssignment GroupAssignment { get; set; } = default!;
    }
}
