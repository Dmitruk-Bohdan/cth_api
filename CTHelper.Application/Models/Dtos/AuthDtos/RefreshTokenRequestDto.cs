using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AuthDtos;

public class RefreshTokenRequestDto
{
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;
}
