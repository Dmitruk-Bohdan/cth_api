using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.PasswordResetToken
{
    public class PasswordResetTokenByTokenHashSpecification : BaseSpecification<CTHelper.Domain.Entities.PasswordResetToken>
    {
        public PasswordResetTokenByTokenHashSpecification(string tokenHash)
        {
            AddCriteria(t => t.TokenHash == tokenHash);
        }
    }
}
