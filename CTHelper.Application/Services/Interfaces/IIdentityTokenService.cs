using CTHelper.Application.Models.User;

namespace CTHelper.Application.Services.Interfaces;

public interface IIdentityTokenService
{
    string ComputeRefreshTokenHash(string token);
    string GenerateAccessToken(UserTokenModel user, Guid sessionJti);
    string GenerateRefreshToken();
    int GetRefreshTokenExpirationDays();
    bool VerifyRefreshToken(string token, string storedTokenHash);
}
