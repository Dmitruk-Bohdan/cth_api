using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AuthDtos;

public class RequestEmailVerificationRequestDto
{
    [JsonPropertyName("userEmail")]
    public string UserEmail { get; set; } = default!;
}
