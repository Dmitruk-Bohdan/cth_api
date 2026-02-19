using CTHelper.Domain.Common.Enums;

namespace CTHelper.Domain.Entities
{
    public class Topic : BaseEntity
    {
        public Subject Subject { get; set; }

        public string Name { get; set; } = default!;

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastUpdateAt { get; set; }

        public ICollection<Problem> Problems { get; set; } = new List<Problem>();
    }
}


