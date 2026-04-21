namespace CTHelper.Application.Models.UserModels;

public class UserLoginModel : UserTokenModel
{
    public bool IsEmailVerified { get; set; }
}
