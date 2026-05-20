using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace CTHelper.UnitTests.Services.ShortTokenService;

public class ShortTokenServiceTests
{
    [Fact]
    public void Get6NumbersToken_Returns6DigitString()
    {
        var secretKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var settings = Options.Create(new TokenSettings
        {
            EmailVerificationTokenBottomBound = 100000,
            EmailVerificationTokenUpperBound = 999999,
            ShortTokenSecretKey = secretKey,
            EmailVerificationTokenLifetimeSeconds = 600,
            AttemptsLimitToValidateEmailVerificationByOneToken = 3,
            PasswordResetTokenLifetimeSeconds = 600,
            AttemptsLimitToValidatePasswordResetByOneToken = 3,
            RefreshTokenSecretKey = secretKey
        });
        var svc = new CTHelper.Infrastructure.Services.Implementations.ShortTokenService(settings);

        var token = svc.Get6NumbersToken();

        Assert.NotNull(token);
        Assert.Equal(6, token.Length);
        Assert.True(int.TryParse(token, out _));
    }

    [Fact]
    public void ComputeHash_ProducesDifferentOutputForDifferentInputs()
    {
        var secretKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var settings = Options.Create(new TokenSettings
        {
            EmailVerificationTokenBottomBound = 100000,
            EmailVerificationTokenUpperBound = 999999,
            ShortTokenSecretKey = secretKey,
            EmailVerificationTokenLifetimeSeconds = 600,
            AttemptsLimitToValidateEmailVerificationByOneToken = 3,
            PasswordResetTokenLifetimeSeconds = 600,
            AttemptsLimitToValidatePasswordResetByOneToken = 3,
            RefreshTokenSecretKey = secretKey
        });
        var svc = new CTHelper.Infrastructure.Services.Implementations.ShortTokenService(settings);

        var hash1 = svc.ComputeHash("123456");
        var hash2 = svc.ComputeHash("654321");

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Verify_SameToken_ReturnsTrue()
    {
        var secretKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var settings = Options.Create(new TokenSettings
        {
            EmailVerificationTokenBottomBound = 100000,
            EmailVerificationTokenUpperBound = 999999,
            ShortTokenSecretKey = secretKey,
            EmailVerificationTokenLifetimeSeconds = 600,
            AttemptsLimitToValidateEmailVerificationByOneToken = 3,
            PasswordResetTokenLifetimeSeconds = 600,
            AttemptsLimitToValidatePasswordResetByOneToken = 3,
            RefreshTokenSecretKey = secretKey
        });
        var svc = new CTHelper.Infrastructure.Services.Implementations.ShortTokenService(settings);

        var token = "123456";
        var hash = svc.ComputeHash(token);
        var result = svc.Verify(token, hash);

        Assert.True(result);
    }
}