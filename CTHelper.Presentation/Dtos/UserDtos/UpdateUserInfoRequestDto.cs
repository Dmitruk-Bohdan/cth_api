using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.UserDtos;

public class UpdateUserInfoRequestDto
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("avatar")]
    public IFormFile AvatarFile { get; set; } = default!;
}
