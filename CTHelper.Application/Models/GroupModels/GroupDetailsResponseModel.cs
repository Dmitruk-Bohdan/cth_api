using CTHelper.Application.Models.UserModels;

namespace CTHelper.Application.Models.Group
{
    public class GroupDetailsResponseModel
    {
        public long GroupId { get; set; }
        public string Name { get; set; } = default!;
        public long SubjectId { get; set; }
        public string SubjectName { get; set; } = default!;
        public long TeacherId { get; set; }
        public string TeacherName { get; set; } = default!;
        public List<UserProfilePreviewModel> Students { get; set; } = new();
        public DateTimeOffset CreatedAt { get; set; }
    }
}
