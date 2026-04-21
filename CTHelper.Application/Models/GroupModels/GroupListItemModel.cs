using CTHelper.Domain.Entities;

namespace CTHelper.Application.Models.Group
{
    public class GroupListItemModel
    {
        public string Name { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public int StudentsCount { get; set; }
    }
}
