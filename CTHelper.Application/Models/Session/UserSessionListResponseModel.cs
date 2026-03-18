using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Session;

public class UserSessionListResponseModel
{
    public Guid Jti { get; set; }
    public ClientType ClientType { get; set; }
    public string? IpAddress { get; set; }
    public string? DeviceInfo { get; set; }
    public DateTimeOffset LastActivityAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
