using CTHelper.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.TestDtos;

public class CreateMixedTestRequestDto
{
    [JsonPropertyName("subjectId")]
    public long SubjectId { get; set; }
    
    [JsonPropertyName("averageDifficult")]
    public ProblemDifficultEnum AverageDifficult { get; set; }
    
    [JsonPropertyName("topicItems")]
    public IEnumerable<MixedTestTopicDto> TopicItems { get; set; } = new List<MixedTestTopicDto>();
}
public class MixedTestTopicDto
{
    [JsonPropertyName("topicId")]
    public long TopicId { get; set; }

    [JsonPropertyName("problemCount")]
    public long ProblemCount { get; set; }
}