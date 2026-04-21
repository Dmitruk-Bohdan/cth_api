using Amazon.S3.Model;
using CTHelper.Application.Models;
using CTHelper.Application.Models.TestModels;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class TestService : ITestService
    {
        private readonly AppDbContext _dbContext;

        public TestService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<OperationResult<Test>> CreateMixedTest(CreateMixedTestRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> CreateTest(CreateTestRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TestDetailsModel>> GetTestDetails(TestDetailsRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<TestListItemModel>>> GetTestList(TeacherTestListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<TestListItemModel>>> GetTestList(MyTestListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<TestListItemModel>>> GetTestList(StudentTestListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TestDetailsModel>> GetTestPreview(TestPreviewRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> RemoveTest(RemoveTestRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateTest(UpdateTestRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
