namespace CTHelper.Application.Models.Assignment
{
    public class AssignTestToGroupRequestModel
    {
        public long TestId { get; set; }
        public long TeacherId { get; set; }
        public long? GroupId { get; set; }
        public DateTimeOffset? Deadline { get; set; }
        public short? AttemptsAllowed { get; set; }
    }
}
