using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.UserSession;

public class ActiveUserSessionByJtiIncludingRefreshTokenSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public ActiveUserSessionByJtiIncludingRefreshTokenSpecification(Guid jti)
    {
        AddCriteria(s => s.Jti == jti);
        AddCriteria(s => s.RevokedAt == null);
        AddInclude(s => s.RefreshToken);
    }
}
