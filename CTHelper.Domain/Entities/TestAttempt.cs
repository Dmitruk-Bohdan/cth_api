using CTHelper.Domain.Common.Enums;

namespace CTHelper.Domain.Entities
{
    public class TestAttempt : BaseEntity
    {
        public long TestId { get; set; }

        public long StudentId { get; set; }

        public TestAttemptStatusType Status { get; set; }

        public int Duration { get; set; }

        public short RawScore { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Test Test { get; set; } = default!;

        public User Student { get; set; } = default!;

        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}
