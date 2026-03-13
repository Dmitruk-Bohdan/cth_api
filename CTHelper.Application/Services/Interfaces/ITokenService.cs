using CTHelper.Application.Models.User;

namespace CTHelper.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(UserTokenModel user, Guid sessionJti);
    string GenerateRefreshToken();
    int GetRefreshTokenExpirationDays();
}
