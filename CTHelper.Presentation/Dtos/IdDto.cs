using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos
{
    public class IdDto
    {
        public IdDto(long id) => Id = id;

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
