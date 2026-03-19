using System.Net;

namespace CTHelper.Application.Models
{
    public class OperationResult
    {
        public OperationResult()
        {
        }
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
    public class OperationResult<T> : OperationResult
    {
        public OperationResult()
        {
        }
        public OperationResult(T payload)
        {
            Payload = payload;
            HttpStatusCode = HttpStatusCode.OK;
        }
        public T? Payload { get; set; }
    }
}
