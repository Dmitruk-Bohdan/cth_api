namespace CTHelper.Application.Models.Assignment
{
    public class StudentScoreByAssignmentResponseModel
    {
        public List<StudentScoreByAssignmentItemResponseDto> Attempts { get; set; } = new();
        public bool? AttemptsLeft { get; set; }
        public short? AveragePercentageScore { get; set; }
        public short? BestPercentageScore { get; set; }
    }

    public class StudentScoreByAssignmentItemResponseDto
    {
        public short? PercentageScore { get; set; }
        public long? AttemptId { get; set; }
        public short Duration { get; set; }
    }
}