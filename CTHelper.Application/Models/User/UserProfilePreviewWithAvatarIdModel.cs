namespace CTHelper.Application.Models.User
{
    public class UserProfilePreviewWithAvatarIdModel
    {
        public long UserId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public long? AvatarId { get; set; }
    }
}
