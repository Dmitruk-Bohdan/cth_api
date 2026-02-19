using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.UserDtos;

public class UpdateUserRequestDto
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
