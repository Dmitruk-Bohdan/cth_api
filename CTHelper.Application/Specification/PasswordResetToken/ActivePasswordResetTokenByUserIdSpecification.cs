using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.PasswordResetToken
{
    public class ActivePasswordResetTokenByUserIdSpecification : BaseSpecification<CTHelper.Domain.Entities.PasswordResetToken>
    {
        public ActivePasswordResetTokenByUserIdSpecification(long userId)
        {
            AddCriteria(t => t.UserId == userId);
            AddCriteria(t => t.UsedAt == null);
        }
    }
}
