using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.UserSession;

public class ActiveUserSessionsByUserIdAndDeviceIdIncludingRefreshTokenSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public ActiveUserSessionsByUserIdAndDeviceIdIncludingRefreshTokenSpecification(long userId, string? deviceId)
    {
        AddCriteria(s => s.UserId == userId);
        AddCriteria(s => s.RefreshToken.DeviceId == deviceId);
        AddCriteria(s => s.RevokedAt == null);
        AddInclude(s => s.RefreshToken);
    }
}
