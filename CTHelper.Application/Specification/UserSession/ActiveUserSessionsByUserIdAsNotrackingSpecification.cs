using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.UserSession;

public class ActiveUserSessionsByUserIdAsNotrackingSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public ActiveUserSessionsByUserIdAsNotrackingSpecification(long userId)
    {
        AddCriteria(s => s.UserId == userId);
        AddCriteria(s => s.RevokedAt == null);
        AsNoTracking = true;
    }
}
