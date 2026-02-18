using СTHelper.Domain.Common.Enums;

namespace СTHelper.Domain.Entities
{
    public class Group : BaseEntity
    {
        public long TeacherId { get; set; }
        public Subject Subject { get; set; }

        public string Name { get; set; } = default!;

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastUpdateAt { get; set; }

        public User Teacher { get; set; } = default!;

        public ICollection<User> Students { get; set; } = new List<User>();
        public List<Assignment> ReceivedAssignments { get; set; } = default!;
    }
}
