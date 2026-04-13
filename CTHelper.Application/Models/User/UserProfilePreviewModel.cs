namespace CTHelper.Application.Models.User
{
    public class UserProfilePreviewModel
    {
        public long UserId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string? AvatarUrl { get; set; }
    }
}
