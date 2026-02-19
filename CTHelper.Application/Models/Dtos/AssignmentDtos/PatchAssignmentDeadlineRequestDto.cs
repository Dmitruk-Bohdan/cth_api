using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AssignmentDtos;

public class PatchAssignmentDeadlineRequestDto
{
    [JsonPropertyName("deadline")]
    public DateTimeOffset? Deadline { get; set; }
}
