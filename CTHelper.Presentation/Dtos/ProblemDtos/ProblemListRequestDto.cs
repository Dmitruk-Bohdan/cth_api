using CTHelper.Application.Common.Enums;
using CTHelper.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.ProblemDtos
{
    public class ProblemListRequestDto
    {

        [JsonPropertyName("subjectId")]
        public long SubjectId { get; set; }

        [JsonPropertyName("searchType")]
        public ProblemSearchTypeEnum SearchType { get; set; }

        [JsonPropertyName("topicId")]
        public long? TopicId { get; set; }

        [JsonPropertyName("type")]
        public ProblemTypeEnum? Type { get; set; }

        [JsonPropertyName("difficulty")]
        public ProblemDifficultEnum? Difficulty { get; set; }

        [JsonPropertyName("searchTerm")]
        public string? SearchTerm { get; set; }

        [JsonPropertyName("isPublished")]
        public bool IsPublished { get; set; }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("onlyMyProblems")]
        public bool OnlyMyProblems { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 10;

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; } = 1;
    }
}
