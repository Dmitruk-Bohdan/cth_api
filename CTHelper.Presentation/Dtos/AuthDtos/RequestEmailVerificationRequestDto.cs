using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AuthDtos;

public class RequestEmailVerificationRequestDto
{
    [JsonPropertyName("userEmail")]
    public string UserEmail { get; set; } = default!;
}
