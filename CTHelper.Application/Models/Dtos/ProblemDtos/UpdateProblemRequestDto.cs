using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.ProblemDtos;

public class UpdateProblemRequestDto
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("topicId")]
    public long? TopicId { get; set; }

    [JsonPropertyName("difficulty")]
    public int? Difficulty { get; set; }

    [JsonPropertyName("correctAnswer")]
    public string? CorrectAnswer { get; set; }
}
