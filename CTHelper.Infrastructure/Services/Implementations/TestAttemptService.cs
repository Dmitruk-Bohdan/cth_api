using CTHelper.Application.Models;
using CTHelper.Application.Models.TestAttemptModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class TestAttemptService : ITestAttemptService
    {
        public Task<OperationResult> CancelTestAttempt(CancelTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> CompleteTestAttempt(CompleteTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TestAttemptDetails>> GetMyAttempt(MyTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetMyAttemptList(MyTestAttemptListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TestAttemptDetails>> GetStudentAttempt(StudentTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetStudentAttemptList(StudentTestAttemptListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> PauseTestAttempt(PauseTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> ResumeTestAttempt(ResumeTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> StartTestAttempt(StartTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
