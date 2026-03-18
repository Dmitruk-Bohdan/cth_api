using CTHelper.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AuthDtos;

public class RegisterUserRequestDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public UserRole Role { get; set; }
}
