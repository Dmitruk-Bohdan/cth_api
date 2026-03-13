using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSession;

public class ActiveUserSessionByJtiAndUserIdSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public ActiveUserSessionByJtiAndUserIdSpecification(long userId, Guid jti)
    {
        AddCriteria(s => s.UserId == userId && s.Jti == jti && s.RevokedAt == null);
    }
}
