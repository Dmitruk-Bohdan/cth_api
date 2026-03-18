using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AssignmentDtos;

public class PatchAssignmentDeadlineRequestDto
{
    [JsonPropertyName("deadline")]
    public DateTimeOffset? Deadline { get; set; }
}
