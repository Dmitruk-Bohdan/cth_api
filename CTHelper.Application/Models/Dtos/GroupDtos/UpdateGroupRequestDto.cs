using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.GroupDtos;

public class UpdateGroupRequestDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
