namespace CTHelper.Application.Models.TestAttemptModels
{
    public class PauseTestAttemptRequestModel : BaseAttemptUserRequestModel
    {
        public IEnumerable<UserAnswerDto>? UserAnswers { get; set; }
    }

    public class UserAnswerDto
    {
        public long UserAnswerId { get; set; }
        public string? Answer { get; set; }
    }
}
