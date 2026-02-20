namespace CTHelper.Application.ServiceInterfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
