using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.User;

public class UserTokenModel
{
    public long UserId { get; set; }
    public string PasswordHash { get; set; } = default!;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Guid SessionJti { get; set; }
}
