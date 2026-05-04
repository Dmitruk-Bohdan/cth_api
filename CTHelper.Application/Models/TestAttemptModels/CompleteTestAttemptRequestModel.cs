namespace CTHelper.Application.Models.TestAttemptModels
{
    public class CompleteTestAttemptRequestModel : BaseAttemptUserRequestModel
    {
        public IEnumerable<UserAnswerDto>? UserAnswers { get; set; }
    }
}
