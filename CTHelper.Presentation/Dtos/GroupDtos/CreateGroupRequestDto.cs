using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Group
{
    public class CreateGroupRequestDto
    {
        [JsonPropertyName("subjectId")]
        public long SubjectId { get; set; }
        
        [JsonPropertyName("groupId")]
        public string GroupName { get; set; } = default!;
    }
}
