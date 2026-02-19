using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.TestDtos;

public class UpdateTestRequestDto
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("timeLimit")]
    public int? TimeLimit { get; set; }

    [JsonPropertyName("passingScore")]
    public int? PassingScore { get; set; }

    [JsonPropertyName("problemIds")]
    public List<long>? ProblemIds { get; set; }
}
