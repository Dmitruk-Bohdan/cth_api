using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos
{
    public class ListRequestModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
