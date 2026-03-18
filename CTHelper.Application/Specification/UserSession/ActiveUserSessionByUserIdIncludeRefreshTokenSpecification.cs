using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.UserSession;

public class ActiveUserSessionByUserIdIncludeRefreshTokenSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public ActiveUserSessionByUserIdIncludeRefreshTokenSpecification(long userId)
    {
        AddCriteria(s => s.UserId == userId);
        AddCriteria(s => s.RevokedAt == null);
        AddInclude(s => s.RefreshToken);
    }
}
