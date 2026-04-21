using CTHelper.Domain.Common.Enums;
using CTHelper.Presentation.Dtos;
using System.Text.Json.Serialization;

namespace CTHelper.Application.Models.TestModels
{
    public class MyTestListRequestDto : PaginatedListRequestDto
    {
        [JsonPropertyName("nameFragment)")]
        public string? NameFragment { get; set; }
        
        [JsonPropertyName("avgDifficult)")]
        public ProblemDifficult? AvgDifficult { get; set; }
        
        [JsonPropertyName("isTraning)")]
        public bool? IsTraning { get; set; }
        
        [JsonPropertyName("type)")]
        public TestType? Type { get; set; }
        
        [JsonPropertyName("maxTaskCount)")]
        public int? MaxTaskCount { get; set; }
        
        [JsonPropertyName("minTaskCount)")]
        public int? MinTaskCount { get; set; }

    }
}
