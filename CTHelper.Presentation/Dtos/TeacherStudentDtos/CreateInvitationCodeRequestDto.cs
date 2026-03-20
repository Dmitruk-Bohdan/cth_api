using CTHelper.Presentation.Common.Attributes;
using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.TeacherStudentDtos
{
    public class CreateInvitationCodeRequestDto
    {
        [JsonPropertyName("usesCount")]
        public short? UsesCount { get; set; }
        
        [JsonPropertyName("expiredAt")]
        [ValidDateTimeOffset]
        public string? ExpiredAt { get; set; } = default!;
    }
}
