using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSession;

public class ActiveUserSessionByUserIdAndDeviceIdSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public ActiveUserSessionByUserIdAndDeviceIdSpecification(long userId, string deviceId)
    {
        AddCriteria(s => s.UserId == userId && s.RevokedAt == null);
    }
}
