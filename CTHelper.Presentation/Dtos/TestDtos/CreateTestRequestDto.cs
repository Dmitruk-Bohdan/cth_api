using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.TestDtos;

public class CreateTestRequestDto
{

    [JsonPropertyName("title")]
    public string Title { get; set; } = default!;

    [JsonPropertyName("subjectId")]
    public long SubjectId { get; set; }

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

public class TestProblemCodeRequestDto
{

    [JsonPropertyName("problemId")]
    public long ProblemId { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; } = default!;
}