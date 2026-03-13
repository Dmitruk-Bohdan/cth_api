using System.Text.Json.Serialization;
using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Dtos.AuthDtos;

public class LoginRequestDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("clientType")]
    public ClientType ClientType { get; set; } = ClientType.Web;

    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; set; }

    [JsonPropertyName("deviceInfo")]
    public string? DeviceInfo { get; set; }

    [JsonPropertyName("deviceId")]
    public string? DeviceId { get; set; }
}
