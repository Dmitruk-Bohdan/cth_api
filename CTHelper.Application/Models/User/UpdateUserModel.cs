namespace CTHelper.Application.Models.User
{
    public class UpdateUserModel
    {
        public long UserId { get; set; }
        public string? Username { get; set; } 
        public long? UserAvatarId { get; set; }
    }
}
