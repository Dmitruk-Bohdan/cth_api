using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.TestModels
{
    public class TeacherTestListRequestDto : BaseTestListRequestDto
    {
        [JsonPropertyName("onlyMyTests")]
        public bool? OnlyMyTests { get; set; }
    }
}
