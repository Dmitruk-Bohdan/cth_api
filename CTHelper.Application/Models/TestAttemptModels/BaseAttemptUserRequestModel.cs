namespace CTHelper.Application.Models.TestAttemptModels
{
    public abstract class BaseAttemptUserRequestModel
    {
        public long UserId { get; set; }
        public long AttemptId { get; set; }
    }
}
