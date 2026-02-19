using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.GroupDtos;

public class CreateGroupRequestDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("invitationCode")]
    public string? InvitationCode { get; set; }
}
