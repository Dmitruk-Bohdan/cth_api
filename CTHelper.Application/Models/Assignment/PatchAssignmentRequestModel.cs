namespace CTHelper.Application.Models.Assignment
{
    public class PatchAssignmentRequestModel
    {
        public long AssignmentId { get; set; }
        public long TeacherId { get; set; }
        public DateTimeOffset? Deadline { get; set; }
        public int? Attempts { get; set; }
    }
}
