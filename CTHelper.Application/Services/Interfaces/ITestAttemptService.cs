using CTHelper.Application.Models;
using CTHelper.Application.Models.TestAttemptModels;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Services.Interfaces
{
    public interface ITestAttemptService
    {
        Task<OperationResult> CancelTestAttempt(CancelTestAttemptRequestModel requestModel);
        Task<OperationResult> CompleteTestAttempt(CompleteTestAttemptRequestModel requestModel);
        Task<OperationResult<TestAttemptDetails>> GetMyAttempt(MyTestAttemptRequestModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetMyAttemptList(MyTestAttemptListRequestModel requestModel);
        Task<OperationResult<TestAttemptDetails>> GetStudentAttempt(StudentTestAttemptRequestModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetStudentAttemptList(StudentTestAttemptListRequestModel requestModel);
        Task<OperationResult> PauseTestAttempt(PauseTestAttemptRequestModel requestModel);
        Task<OperationResult> ResumeTestAttempt(ResumeTestAttemptRequestModel requestModel);
        Task<OperationResult> StartTestAttempt(StartTestAttemptRequestModel requestModel);
    }
}
