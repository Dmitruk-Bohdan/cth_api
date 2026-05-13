using CTHelper.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.ProblemDtos;

public class UpdateProblemRequestDto
{

    [JsonPropertyName("difficulty")]
    public ProblemDifficultEnum Difficulty { get; set; }

    [JsonPropertyName("statement")]
    public string Statement { get; set; } = default!;

    [JsonPropertyName("correctAnswer")]
    public string correctAnswer { get; set; } = default!;

    [JsonPropertyName("explanation")]
    public string Explanation { get; set; } = default!;

    [JsonPropertyName("isPublished")]
    public bool IsPublished { get; set; }

    [JsonPropertyName("isPublic")]
    public bool IsPublic { get; set; }
}
