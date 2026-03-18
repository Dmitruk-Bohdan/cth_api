using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.UserSession;

public class ActiveUserSessionByJtiSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public ActiveUserSessionByJtiSpecification(Guid jti)
    {
        AddCriteria(s => s.Jti == jti);
        AddCriteria(s => s.RevokedAt == null);
    }
}
