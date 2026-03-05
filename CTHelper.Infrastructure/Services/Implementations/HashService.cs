using CTHelper.Application.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CTHelper.Infrastructure.Services.Implementations;
public class HashService : IHashService
{
    public string Get128Hash(string password)
    {
        if (password == null) throw new ArgumentNullException(nameof(password));

        using (var sha512 = SHA512.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);

            byte[] hashBytes = sha512.ComputeHash(bytes);

            StringBuilder builder = new StringBuilder(128);
            foreach (var b in hashBytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }
    }
}