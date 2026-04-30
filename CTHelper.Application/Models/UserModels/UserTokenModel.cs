using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.UserModels;

public class UserTokenModel
{
    public long UserId { get; set; }
    public string PasswordHash { get; set; } = default!;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRoleEnum Role { get; set; }
    public Guid SessionJti { get; set; }
}
