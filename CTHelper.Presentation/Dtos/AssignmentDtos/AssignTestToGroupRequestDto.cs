using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AssignmentDtos;

public class AssignTestToGroupRequestDto
{
    [JsonPropertyName("testId")]
    public long TestId { get; set; }

    [JsonPropertyName("groupId")]
    public long GroupId { get; set; }

    [JsonPropertyName("expired_at")]
    public DateTimeOffset? Deadline { get; set; }

    [JsonPropertyName("attempts_allowed")]
    public short? AttemptsAllowed { get; set; }
}
