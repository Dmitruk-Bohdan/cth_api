namespace CTHelper.Application.Models.TeacherStudent
{
    public class CreateInvitationCodeResponseModel
    {
        public long CodeId { get; set; }
        public long TeacherId { get; set; }
        public string Code { get; set; } = default!;
        public short? UsesLeft { get; set; }
        public DateTimeOffset? ExpiredAt { get; set; }
    }
}
