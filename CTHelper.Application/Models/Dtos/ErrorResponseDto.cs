using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.Dtos
{
    public class ErrorResponseDto
    {
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; } = default!;

        [JsonPropertyName("errorName")]
        public string ErrorMessage { get; set; } = default!;
    }
}
