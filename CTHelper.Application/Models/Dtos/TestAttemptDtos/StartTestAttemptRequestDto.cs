using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.TestAttemptDtos;

public class StartTestAttemptRequestDto
{
    [JsonPropertyName("testId")]
    public long TestId { get; set; }
}
