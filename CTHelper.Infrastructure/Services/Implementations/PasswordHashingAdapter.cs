using CTHelper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

public class PasswordHasherAdapter : IPasswordHashingService
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null, password);
    }

    public bool Verify(string password, string hash)
    {
        var result = _hasher.VerifyHashedPassword(null, hash, password);

        return result != PasswordVerificationResult.Failed;
    }
}