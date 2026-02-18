using СTHelper.Domain.Common.Enums;

namespace СTHelper.Domain.Entities
{
    public class TeacherStudent : BaseEntity
    {
        public long TeacherId { get; set; }

        public long StudentId { get; set; }

        public TeacherStudentStatus Status { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastUpdateAt { get; set; }

        public User Teacher { get; set; } = default!;

        public User Student { get; set; } = default!;

    }
}


