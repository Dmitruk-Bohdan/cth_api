using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AuthDtos;

public class LogoutRequestDto
{
    [JsonPropertyName("userId")]
    public long UserId { get; set; }

    [JsonPropertyName("sessionJti")]
    public Guid? SessionJti { get; set; }
}
