using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.RefreshToken;

public class RefreshTokensBySessionIdSpecification : BaseSpecification<Domain.Entities.RefreshToken>
{
    public RefreshTokensBySessionIdSpecification(long sessionId)
    {
        AddCriteria(rt => rt.SessionId == sessionId);
    }
}
