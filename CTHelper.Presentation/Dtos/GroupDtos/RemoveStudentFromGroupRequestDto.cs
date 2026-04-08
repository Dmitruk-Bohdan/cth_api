using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Group
{
    public class RemoveStudentFromGroupRequestDto
    {
        [JsonPropertyName("studentId")]
        public long StudentId { get; set; }

        [JsonPropertyName("groupId")]
        public long GroupId { get; set; }
    }
}
