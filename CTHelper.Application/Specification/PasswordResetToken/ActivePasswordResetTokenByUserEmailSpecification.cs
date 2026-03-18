using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.PasswordResetToken
{
    public class ActivePasswordResetTokenByUserEmailSpecification : BaseSpecification<CTHelper.Domain.Entities.PasswordResetToken>
    {
        public ActivePasswordResetTokenByUserEmailSpecification(long userId)
        {
            AddCriteria(t => t.UserId == userId);
            AddCriteria(t => t.UsedAt == null);
        }
    }
}
