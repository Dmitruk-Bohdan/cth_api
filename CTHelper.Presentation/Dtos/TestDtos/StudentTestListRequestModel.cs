using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.TestModels
{
    public class StudentTestListRequestDto : BaseTestListRequestDto
    {
        [JsonPropertyName("assignedToMe")]
        public bool? AssignedToMe { get; set; }
    }
}
