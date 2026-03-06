using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Domain.Specification
{
    public class EmailConfirmationActiveTokenByUserIdAsNoTrackingSpecification : BaseSpecification<EmailVerificationToken>
    {
        public EmailConfirmationActiveTokenByUserIdAsNoTrackingSpecification(long userId)
        {
            AddCriteria(t => t.UserId == userId);
            AddCriteria(t => t.VerifiedAt == null);
            AsNoTracking = true;
        }
    }
}
