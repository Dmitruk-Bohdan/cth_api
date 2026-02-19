using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AuthDtos;

public class ConfirmPasswordResetRequestDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; } = string.Empty;
}
