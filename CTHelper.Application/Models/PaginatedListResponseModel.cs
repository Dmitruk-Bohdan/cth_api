using System.Text.Json.Serialization;

namespace CTHelper.Presentation.Dtos
{
    public class PaginatedListResponseModel<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalPagesCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}