using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.RefreshToken;

public class ActiveRefreshTokenByJtiSpecification : BaseSpecification<Domain.Entities.RefreshToken>
{
    public ActiveRefreshTokenByJtiSpecification(Guid jti)
    {
        AddCriteria(rt => rt.Session.Jti == jti && rt.RevokedAt == null);
    }
}
