namespace CTHelper.Application.Models.Assignment
{
    public class GetStudentAssignmentScoreModel
    {
        public long AssignmentId { get; set; }
        public long? TeacherId { get; set; }
        public long? StudentId { get; set; }
    }
}
