using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Domain.Specification
{
    public class EmailConfirmationActiveTokenByUserIdSpecification : BaseSpecification<EmailVerificationToken>
    {
        public EmailConfirmationActiveTokenByUserIdSpecification(long userId)
        {
            AddCriteria(t => t.UserId == userId);
            AddCriteria(t => t.VerifiedAt == null);
        }
    }
}
