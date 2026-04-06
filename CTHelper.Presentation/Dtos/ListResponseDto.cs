using System.Collections;
using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos
{
    public class ListResponseDto<T>
    {
        public ListResponseDto(IEnumerable<T>? items)
        {
            Items = items ?? new List<T>();
        }

        [JsonPropertyName("items")]
        public IEnumerable<T>? Items { get; set; } = new List<T>();
    }
}
