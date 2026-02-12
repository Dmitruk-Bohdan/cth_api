namespace СTHelper.Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public long TeacherStudentId { get; set; }

        public long TestId { get; set; }

        public DateTimeOffset ExpiredAt { get; set; }

        public short AttemptsLeft { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastUpdateAt { get; set; }

        public TeacherStudent TeacherStudent { get; set; } = default!;

        public Test Test { get; set; } = default!;
    }
}
