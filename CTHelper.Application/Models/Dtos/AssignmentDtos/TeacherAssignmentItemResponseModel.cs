using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AssignmentDtos
{
    public class TeacherAssignmentItemResponseModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("testId")]
        public long TestId { get; set; }

        [JsonPropertyName("testName")]
        public string TestName { get; set; } = default!;

        [JsonPropertyName("studentId")]
        public long StudentId { get; set; }

        [JsonPropertyName("studentName")]
        public string StudentName { get; set; } = default!;

        [JsonPropertyName("groupId")]
        public long? GroupId { get; set; }

        [JsonPropertyName("groupName")]
        public string? GroupName { get; set; } = default!;

        [JsonPropertyName("expiredAt")]
        public string? ExpiredAt { get; set; } // ISO 8601 (2026-02-18T18:17:01Z)

    }
}
