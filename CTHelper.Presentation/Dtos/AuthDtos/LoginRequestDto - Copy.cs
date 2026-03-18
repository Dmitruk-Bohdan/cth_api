using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AuthDtos;

public class LoginResponseDto
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;
}
