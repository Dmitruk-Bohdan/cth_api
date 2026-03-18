using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.GroupDtos;

public class UpdateGroupRequestDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
