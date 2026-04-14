using CTHelper.Application.Models.Test;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.TestAttemptDtos;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public interface ITestService
    {
        Task<PaginatedListResponseModel<TestPreviewModel>> GetTestList(TestListRequestModel request);
    }
}