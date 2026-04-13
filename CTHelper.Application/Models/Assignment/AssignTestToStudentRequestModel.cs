using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Assignment
{
    public class AssignTestToStudentRequestModel
    {
        public long TestId { get; set; }
        public long TeacherId { get; set; }
        public long StudentId { get; set; }
        public DateTimeOffset? Deadline { get; set; }
        public short? AttemptsAllowed { get; set; }
    }
}
