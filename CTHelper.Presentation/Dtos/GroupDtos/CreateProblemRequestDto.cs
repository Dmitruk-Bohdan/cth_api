using CTHelper.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.GroupDtos;

public class CreateProblemRequestDto
{
    [JsonPropertyName("type")]
    public ProblemTypeEnum Type { get; set; }

    [JsonPropertyName("difficulty")]
    public ProblemDifficultEnum Difficulty { get; set; }

    [JsonPropertyName("statement")]
    public string Statement { get; set; } = default!;

    [JsonPropertyName("correctAnswer")]
    public string correctAnswer { get; set; } = default!;

    [JsonPropertyName("explanation")]
    public string Explanation { get; set; } = default!;

    [JsonPropertyName("topicId")]
    public long TopicId { get; set; }

    [JsonPropertyName("isPublished")]
    public bool IsPublished { get; set; }

    [JsonPropertyName("isPublic")]
    public bool IsPublic { get; set; }
}
