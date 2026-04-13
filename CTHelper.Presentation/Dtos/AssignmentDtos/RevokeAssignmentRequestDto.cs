using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AssignmentDtos
{
    public class RevokeAssignmentRequestDto
    {
        [JsonPropertyName("assignmentId")]
        public long AssignmentId { get; set; }
    }
}
