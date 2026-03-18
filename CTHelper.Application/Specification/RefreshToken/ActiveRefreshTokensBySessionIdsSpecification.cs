using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.RefreshToken;

public class ActiveRefreshTokensBySessionIdsSpecification : BaseSpecification<Domain.Entities.RefreshToken>
{
    public ActiveRefreshTokensBySessionIdsSpecification(List<long> sessionIds)
    {
        AddCriteria(rt => sessionIds.Contains(rt.SessionId) && rt.RevokedAt == null);
    }
}
