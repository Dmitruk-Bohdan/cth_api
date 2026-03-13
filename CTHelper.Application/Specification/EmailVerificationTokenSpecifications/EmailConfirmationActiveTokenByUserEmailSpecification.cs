using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.EmailVerificationTokenSpecifications
{
    public class EmailConfirmationActiveTokenByUserEmailSpecification : BaseSpecification<EmailVerificationToken>
    {
        public EmailConfirmationActiveTokenByUserEmailSpecification(string email, long userId)
        {
            AddCriteria(t => t.UserId == userId);
            AddCriteria(t => t.VerifiedAt == null);
        }
    }
}
