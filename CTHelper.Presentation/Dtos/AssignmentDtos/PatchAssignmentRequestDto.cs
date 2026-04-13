using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AssignmentDtos;

public class PatchAssignmentRequestDto
{
    [JsonPropertyName("deadline")]
    public long AssignmentId { get; set; }

    [JsonPropertyName("deadline")]
    public DateTimeOffset? Deadline { get; set; }

    [JsonPropertyName("attempts")]
    public int? Attempts { get; set; }
}
