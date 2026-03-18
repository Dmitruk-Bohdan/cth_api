using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos.AssignmentDtos
{
    public class AssignmentListItemResponseDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("testName")]
        public string TestName { get; set; } = default!;

        [JsonPropertyName("expiredAt")]
        public string? ExpiredAt { get; set; } // ISO 8601 (2026-02-18T18:17:01Z)
    }
}
