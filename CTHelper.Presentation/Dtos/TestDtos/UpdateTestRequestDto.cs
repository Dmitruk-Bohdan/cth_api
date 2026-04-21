using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.TestDtos;

public class UpdateTestRequestDto
{

    [JsonPropertyName("title")]
    public string Title { get; set; } = default!;

    [JsonPropertyName("isTraning")]
    public bool IsTraning { get; set; }

    [JsonPropertyName("isPublished")]
    public bool IsPublished { get; set; }

    [JsonPropertyName("isPublic")]
    public bool IsPublic { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("attemptsCount")]
    public int? AttemptsCount { get; set; }

    [JsonPropertyName("problemItems")]
    public IEnumerable<TestProblemCodeRequestDto> TestProblemList { get; set; } = new List<TestProblemCodeRequestDto>();
}