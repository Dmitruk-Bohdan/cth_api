using CTHelper.Application.Services.Interfaces;
using CTHelper.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class ShortTokenService : IShortTokenService
    {
        private readonly TokenSettings _settings;
        private readonly byte[] _secretKey;

        public ShortTokenService(IOptions<TokenSettings> settings)
        {
            _settings = settings.Value;
            _secretKey = Convert.FromBase64String(_settings.ShortTokenSecretKey);
        }

        public string Get6NumbersToken()
        {
            var tokenAsNumber = RandomNumberGenerator.GetInt32(
                _settings.EmailVerificationTokenBottomBound,
                _settings.EmailVerificationTokenUpperBound);
            return tokenAsNumber.ToString("D6");
        }
        public string ComputeHash(string token)
        {
            using var hmac = new HMACSHA256(_secretKey);

            byte[] hash = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(token)
            );

            return Convert.ToBase64String(hash);
        }
        public bool Verify(string token, string storedHash)
        {
            string computed = ComputeHash(token);

            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(storedHash),
                Convert.FromBase64String(computed)
            );
        }
    }
}
