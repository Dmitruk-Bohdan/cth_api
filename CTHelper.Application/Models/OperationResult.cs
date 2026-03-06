namespace CTHelper.Application.Models
{
    public class OperationResult
    {
        public string? ErrorMessage { get; set; }
        public int? ErrorCode { get; set; }
    }
    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; set; }
    }
}
