using CTHelper.Application.Models.UserModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CTHelper.Infrastructure.Services.Implementations;

public class IdentityTokenService : IIdentityTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _refreshTokenSecretKey;

    public IdentityTokenService(
        IOptions<JwtSettings> jwtSettings,
        IOptions<TokenSettings> tokenSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _refreshTokenSecretKey = Convert.FromBase64String(tokenSettings.Value.ShortTokenSecretKey);
    }

    public string GenerateAccessToken(UserTokenModel user, Guid sessionJti)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, sessionJti.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string ComputeRefreshTokenHash(string token)
    {
        using var hmac = new HMACSHA256(_refreshTokenSecretKey);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }

    public bool VerifyRefreshToken(string token, string storedTokenHash)
    {
        using var hmac = new HMACSHA256(_refreshTokenSecretKey);
        var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
        return CryptographicOperations.FixedTimeEquals(computed, Convert.FromBase64String(storedTokenHash));
    }

    public int GetRefreshTokenExpirationDays() => _jwtSettings.RefreshTokenExpirationDays;
}
