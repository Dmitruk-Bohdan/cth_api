using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.TeacherStudentDtos;

public class    CreateBindingRequestDto
{

    [JsonPropertyName("code")]
    public string Code { get; set; } = default!;
}
