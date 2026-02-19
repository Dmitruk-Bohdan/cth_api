using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CTHelper.Application.Models.Dtos
{
    public class IdDto
    {
        public IdDto(long id) => Id = id;

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
