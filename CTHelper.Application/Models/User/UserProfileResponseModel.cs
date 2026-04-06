using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.User
{
    public class UserProfileResponseModel
    {
        public long UserId {  get; set; }
        public string Username { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string? AvatarUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
