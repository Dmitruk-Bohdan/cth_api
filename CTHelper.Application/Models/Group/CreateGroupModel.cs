namespace CTHelper.Application.Models.Group
{
    public class CreateGroupModel
    {
        public long TeacherId { get; set; }
        public long SubjectId { get; set; }
        public string GroupName { get; set; } = default!;
    }
}
