using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AssignmentDtos
{
    public class StudentAssignmentItemResponseModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("testId")]
        public long TestId { get; set; }

        [JsonPropertyName("testName")]
        public string TestName { get; set; } = default!;

        [JsonPropertyName("teacherName")]
        public string TeacherName { get; set; } = default!;

        [JsonPropertyName("teacherId")]
        public long TeacherId { get; set; }

        [JsonPropertyName("attempteLeft")]
        public short AttempteLeft { get; set; }

        [JsonPropertyName("expiredAt")]
        public string? ExpiredAt { get; set; } // ISO 8601 (2026-02-18T18:17:01Z)

    }
}
