using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AuthDtos;

public class RequestPasswordResetRequestDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
