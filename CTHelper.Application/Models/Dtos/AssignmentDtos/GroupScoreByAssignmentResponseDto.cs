

using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AssignmentDtos
{
    public class GroupScoreByAssignmentResponseDto
    {
        [JsonPropertyName("averagePercentageScore")]
        public short? AveragePercentageScore { get; set; }

        [JsonPropertyName("percentageOfСompletion")]
        public short? PercentageOfСompletion { get; set; }
    }

    public class GroupMemberScoreByAssignmentResponseDto
    {
        [JsonPropertyName("studentId")]
        public long StudentId { get; set; } = default!;

        [JsonPropertyName("studentName")]
        public string StudentName { get; set; } = default!;

        [JsonPropertyName("isPassed")]
        public bool IsPassed { get; set; }

        [JsonPropertyName("percentageScore")]
        public short? PercentageScore { get; set; }

        [JsonPropertyName("attemptId")]
        public long? AttemptId { get; set; }
    }
}
