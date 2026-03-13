using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSession;

public class ActiveUserSessionsByUserIdSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public ActiveUserSessionsByUserIdSpecification(long userId)
    {
        AddCriteria(s => s.UserId == userId && s.RevokedAt == null);
    }
}
