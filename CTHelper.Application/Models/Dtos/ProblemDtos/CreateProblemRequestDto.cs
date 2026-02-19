using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.ProblemDtos;

public class CreateProblemRequestDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("topicId")]
    public long TopicId { get; set; }

    [JsonPropertyName("difficulty")]
    public int Difficulty { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("correctAnswer")]
    public string? CorrectAnswer { get; set; }
}
