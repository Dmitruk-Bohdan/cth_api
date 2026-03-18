using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AuthDtos;

public class LogoutRequestDto
{
    [JsonPropertyName("userId")]
    public long UserId { get; set; }

    [JsonPropertyName("sessionJti")]
    public Guid? SessionJti { get; set; }
}
