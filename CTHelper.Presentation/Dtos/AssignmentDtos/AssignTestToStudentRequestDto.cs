using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AssignmentDtos
{
    public class AssignTestToStudentRequestDto
    {
        [JsonPropertyName("testId")]
        public long TestId { get; set; }

          [JsonPropertyName("studentId")]
        public long StudentId { get; set; }

        [JsonPropertyName("expired_at")]
        public DateTimeOffset? Deadline { get; set; }

        [JsonPropertyName("attempts_allowed")]
        public short? AttemptsAllowed { get; set; }
    }
}
