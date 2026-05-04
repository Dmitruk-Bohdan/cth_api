using CTHelper.Application.Models;
using CTHelper.Application.Models.TestModels;
using CTHelper.Domain.Entities;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public interface ITestService
    {
        Task<OperationResult<Test>> CreateMixedTest(CreateMixedTestRequestModel requestModel);
        Task<OperationResult> CreateTest(CreateTestRequestModel requestModel);
        Task<OperationResult<TestDetailsModel>> GetTestDetails(TestDetailsRequestModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<TestListItemModel>>> GetTestList(TeacherTestListRequestModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<TestListItemModel>>> GetTestList(MyTestListRequestModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<TestListItemModel>>> GetTestList(StudentTestListRequestModel requestModel);
        Task<OperationResult<TestPreviewModel>> GetTestPreview(TestPreviewRequestModel requestModel);
        Task<OperationResult> RemoveTest(RemoveTestRequestModel requestModel);
        Task<OperationResult> UpdateTest(UpdateTestRequestModel requestModel);
    }
}