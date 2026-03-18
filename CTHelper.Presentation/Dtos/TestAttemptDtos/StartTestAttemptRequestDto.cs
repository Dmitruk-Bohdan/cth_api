using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.TestAttemptDtos;

public class StartTestAttemptRequestDto
{
    [JsonPropertyName("testId")]
    public long TestId { get; set; }
}
