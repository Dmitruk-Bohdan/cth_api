using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.User;

public class UserSessionWithDeviceIdModel
{
    public long SessionId { get; set; }
    public Guid Jti { get; set; }
    public ClientType ClientType { get; set; }
    public string? IpAddress { get; set; }
    public string? DeviceInfo { get; set; }
    public string? DeviceId { get; set; }
    public DateTimeOffset LastActivityAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
}
