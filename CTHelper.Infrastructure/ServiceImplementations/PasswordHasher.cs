using CTHelper.Application.ServiceInterfaces;
using System.Security.Cryptography;
using System.Text;

namespace CTHelper.Infrastructure.ServiceImplementations;
public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
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