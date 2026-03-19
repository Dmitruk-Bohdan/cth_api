namespace CTHelper.Domain.Entities
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; } = default!;

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastUpdateAt { get; set; }

        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}
