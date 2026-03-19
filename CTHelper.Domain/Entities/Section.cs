using CTHelper.Domain.Common.Enums;

namespace CTHelper.Domain.Entities
{
    public class Section : BaseEntity
    {
        public string Name { get; set; } = default!;
        public long SubjectId { get; set; } = default!;
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdateAt { get; set; }
        public Subject Subject { get; set; } = default!;
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}


