namespace CTHelper.Application.Models.User;

public class UserPasswordModel
{
    public long Id { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}
