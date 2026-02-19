using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AssignmentDtos
{
    public class TeacherAssignmentListResponseDto
    {
        [JsonPropertyName("assignments")]
        public List<AssignmentListItemResponseDto> Assignments { get; set; } = new();
    }

    public class TeacherAssignmentListItemResponseDto : AssignmentListItemResponseDto
    {
        [JsonPropertyName("groupName")] 
        public string? GroupName { get; set; }

        [JsonPropertyName("studentName")]
        public string? StudentName { get; set; }
    }
}
