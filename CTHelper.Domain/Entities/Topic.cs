using CTHelper.Domain.Common.Enums;

namespace CTHelper.Domain.Entities
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; } = default!;
        public long SectionId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdateAt { get; set; }
        public Section Section { get; set; } = default!;
        public ICollection<Problem> Problems { get; set; } = new List<Problem>();
    }
}


