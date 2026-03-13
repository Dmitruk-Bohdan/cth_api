using CTHelper.Application.Models.User;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSession;

public class UserSessionsByUserIdSpecification : BaseSpecification<Domain.Entities.UserSession, UserSessionWithDeviceIdModel>
{
    public UserSessionsByUserIdSpecification(long userId)
    {
        AddCriteria(s => s.UserId == userId);
        ApplySelector(s => new UserSessionWithDeviceIdModel
        {
            SessionId = s.Id,
            Jti = s.Jti,
            ClientType = s.ClientType,
            IpAddress = s.IpAddress,
            DeviceInfo = s.DeviceInfo,
            DeviceId = "", // Will be populated from RefreshToken
            LastActivityAt = s.LastActivityAt,
            CreatedAt = s.CreatedAt,
            RevokedAt = s.RevokedAt
        });
    }
}
