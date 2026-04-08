using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos
{
    public class PaginatedListRequestModel
    {
        [JsonPropertyName("page")]
        public int PageNumber { get; set; } = 1;

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 20;
    }
}
