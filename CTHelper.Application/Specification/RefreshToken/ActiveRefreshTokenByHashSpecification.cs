using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.RefreshToken;

public class ActiveRefreshTokenByHashSpecification : BaseSpecification<Domain.Entities.RefreshToken>
{
    public ActiveRefreshTokenByHashSpecification(string tokenHash)
    {
        AddCriteria(rt => rt.TokenHash == tokenHash && rt.RevokedAt == null);
    }
}
