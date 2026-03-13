namespace CTHelper.Application.Services.Interfaces
{
    public interface IShortTokenService
    {
        string ComputeHash(string token);
        string Get6NumbersToken();
        bool Verify(string token, string storedHash);
    }
}