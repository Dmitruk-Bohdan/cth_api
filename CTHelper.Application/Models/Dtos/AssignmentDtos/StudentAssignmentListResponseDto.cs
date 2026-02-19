using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AssignmentDtos
{
    public class StudentAssignmentListResponseDto
    {
        [JsonPropertyName("assignments")]
        public List<TeacherAssignmentListItemResponseDto> Assignments { get; set; } = new();
    }

    public class StudentAssignmentListItemResponseDto : AssignmentListItemResponseDto
    {
        [JsonPropertyName("teacherName")]
        public string TeacherName { get; set; } = default!;

        [JsonPropertyName("isPassed")]
        public bool isPassed { get; set; } = default!;        
    }
}
