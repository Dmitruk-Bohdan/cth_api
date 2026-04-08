using CTHelper.Presentation.Dtos;
using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Group
{
    public class MyGroupListRequestDto : PaginatedListRequestDto
    {
        [JsonPropertyName("subjectId")]
        public long SubjectId { get; set; }

        [JsonPropertyName("groupId")]
        public string? GroupName { get; set; } = default!;
    }
}
