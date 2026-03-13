using System.Net;
using System.Text.Json.Serialization;

namespace CTHelper.Application.Models
{
    public class OperationResult
    {
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
    public class OperationResult<T> : OperationResult
    {
        public T? Payload { get; set; }
    }
}
