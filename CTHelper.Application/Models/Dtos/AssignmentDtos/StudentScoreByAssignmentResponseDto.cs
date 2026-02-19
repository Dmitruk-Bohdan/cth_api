using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.AssignmentDtos
{
    public class StudentScoreByAssignmentResponseDto
    {
        [JsonPropertyName("attempts")]
        public List<StudentScoreByAssignmentItemResponseDto> Attempts { get; set; } = new();
          
        [JsonPropertyName("attemptsLeft")]
        public bool? AttemptsLeft { get; set; }

        [JsonPropertyName("averagePercentageScore")]
        public short? AveragePercentageScore { get; set; }

        [JsonPropertyName("bestPercentageScore")]
        public short? BestPercentageScore { get; set; }
    }

    public class StudentScoreByAssignmentItemResponseDto
    {
        [JsonPropertyName("percentageScore")]
        public short? PercentageScore { get; set; }

        [JsonPropertyName("attemptId")]
        public long? AttemptId { get; set; }

        [JsonPropertyName("duration")]
        public short Duration { get; set; }
    }
}
