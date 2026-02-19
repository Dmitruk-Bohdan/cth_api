using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AuthDtos;

public class ConfirmEmailVerificationRequestDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
}
