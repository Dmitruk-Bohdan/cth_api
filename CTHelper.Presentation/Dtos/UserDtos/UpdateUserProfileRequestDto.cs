using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.UserDtos;

public class UpdateUserProfileRequestDto
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }
}
