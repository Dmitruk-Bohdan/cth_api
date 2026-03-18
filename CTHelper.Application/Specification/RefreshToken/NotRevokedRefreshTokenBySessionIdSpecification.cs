using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.RefreshToken;

public class NotRevokedRefreshTokenBySessionIdSpecification : BaseSpecification<Domain.Entities.RefreshToken>
{
    public NotRevokedRefreshTokenBySessionIdSpecification(long sessionId)
    {
        AddCriteria(rt => rt.SessionId == sessionId);
        AddCriteria(rt => rt.RevokedAt == null);
    }
}
