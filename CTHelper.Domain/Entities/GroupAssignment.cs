namespace CTHelper.Domain.Entities
{
    public class GroupAssignment : BaseEntity
    {
        public long TeacherId { get; set; }
        public long? GroupId { get; set; }
        public long TestId { get; set; }
        public DateTimeOffset ExpiredAt { get; set; }
        public short? DefaultAttemptsAllowed { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdateAt { get; set; }

        public User Teacher { get; set; } = default!;
        public Group Group { get; set; } = default!;
        public Test Test { get; set; } = default!;

        public List<StudentAssignment> StudentAssignments { get; set; } = new();
    }
}
