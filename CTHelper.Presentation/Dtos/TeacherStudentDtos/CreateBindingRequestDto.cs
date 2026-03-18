using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.TeacherStudentDtos;

public class CreateBindingRequestDto
{
    [JsonPropertyName("studentId")]
    public long StudentId { get; set; }
}
