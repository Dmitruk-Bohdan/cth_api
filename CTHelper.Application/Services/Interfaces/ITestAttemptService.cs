using CTHelper.Application.Models;
using CTHelper.Application.Models.TestAttemptModels;
using CTHelper.Application.Models.TestModels;
using CTHelper.Presentation.Dtos;

public interface ITestAttemptService
{
    Task<OperationResult> CancelTestAttempt(CancelTestAttemptRequestModel requestModel);
    Task<OperationResult<CompleteTestAttemptResponseModel>> CompleteTestAttempt(CompleteTestAttemptRequestModel requestModel);
    Task<OperationResult<TestAttemptDetails>> GetMyAttempt(MyTestAttemptRequestModel requestModel);
    Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetMyAttemptList(MyTestAttemptListRequestModel requestModel);
    Task<OperationResult<TestAttemptDetails>> GetStudentAttempt(StudentTestAttemptRequestModel requestModel);
    Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetStudentAttemptList(StudentTestAttemptListRequestModel requestModel);
    Task<OperationResult> PauseTestAttempt(PauseTestAttemptRequestModel requestModel);
    Task<OperationResult<TestPassingResponseModel>> ResumeTestAttempt(ResumeTestAttemptRequestModel requestModel);
    Task<OperationResult<TestPassingResponseModel>> StartTestAttempt(StartTestAttemptRequestModel requestModel);
}