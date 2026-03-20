using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.User;

public class UserLoginModel : UserTokenModel
{
    public bool IsEmailVerified { get; set; }
}
