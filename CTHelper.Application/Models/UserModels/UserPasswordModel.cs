namespace CTHelper.Application.Models.UserModels;

public class UserPasswordModel
{
    public long Id { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}
