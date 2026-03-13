using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.UserSession;

public class ActiveUserSessionByHashIncludingRefreshTokenSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public ActiveUserSessionByHashIncludingRefreshTokenSpecification(string hash)
    {
        AddCriteria(s => s.RefreshToken.TokenHash == hash);
        AddCriteria(s => s.RevokedAt == null);
        AddInclude(s => s.RefreshToken);
    }
}
