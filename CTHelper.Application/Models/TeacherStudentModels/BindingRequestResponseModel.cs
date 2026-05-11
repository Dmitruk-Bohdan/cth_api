namespace CTHelper.Application.Models.TeacherStudent
{
    public class BindingRequestResponseModel
    {
        public long BindingRequestId { get; set; }
        public long StudentId { get; set; }
        public string StudentUsername { get; set; } = default!;
        public long? StudentAvatarId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}