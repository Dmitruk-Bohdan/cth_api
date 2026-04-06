using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.TeacherStudentDtos;

public class AcceptBindingRequestDto
{
    [JsonPropertyName("requestId")]
    public long BindingRequestId { get; set; }
}
