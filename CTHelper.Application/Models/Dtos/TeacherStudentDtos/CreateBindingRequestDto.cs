using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos.TeacherStudentDtos;

public class CreateBindingRequestDto
{
    [JsonPropertyName("studentId")]
    public long StudentId { get; set; }
}
