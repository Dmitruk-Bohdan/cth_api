using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AssignmentDtos;

public class CreateAssignmentRequestDto
{
    [JsonPropertyName("testId")]
    public long TestId { get; set; }

    [JsonPropertyName("groupId")]
    public long? GroupId { get; set; }

    [JsonPropertyName("studentId")]
    public long? StudentId { get; set; }

    [JsonPropertyName("expired_at")]
    public DateTimeOffset? Deadline { get; set; }

    [JsonPropertyName("attempts_allowed")]
    public short? AttemptsAllowed { get; set; }
}
