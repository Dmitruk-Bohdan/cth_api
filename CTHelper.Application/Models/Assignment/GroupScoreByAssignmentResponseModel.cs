namespace CTHelper.Application.Models.Assignment
{
    public class GroupScoreByAssignmentResponseModel
    {
        public short? AveragePercentageScore { get; set; }
        public short? PercentageOfСompletion { get; set; }
    }

    public class GroupMemberScoreByAssignmentResponseDto
    {
        public long StudentId { get; set; } = default!;
        public string StudentName { get; set; } = default!;
        public bool IsPassed { get; set; }
        public short? PercentageScore { get; set; }
        public long? AttemptId { get; set; }
    }
}
