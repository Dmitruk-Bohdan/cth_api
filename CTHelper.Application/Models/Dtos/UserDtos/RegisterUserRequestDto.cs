using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.UserDtos;

public class RegisterUserRequestDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
