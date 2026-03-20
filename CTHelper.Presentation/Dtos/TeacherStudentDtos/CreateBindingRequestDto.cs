using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.TeacherStudentDtos;

public class    CreateBindingRequestDto
{
    [JsonPropertyName("requestId")]
    public long StudentId { get; set; }

    [JsonPropertyName("code")]
    public long Code { get; set; }
}
