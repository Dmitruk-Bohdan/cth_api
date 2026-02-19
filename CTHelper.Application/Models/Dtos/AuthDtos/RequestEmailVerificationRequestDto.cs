using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AuthDtos;

public class RequestEmailVerificationRequestDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
