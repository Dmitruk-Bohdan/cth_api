using Amazon.S3.Model;
using CTHelper.Application.Models.Test;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.TestAttemptDtos;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class TestService
    {
        private readonly AppDbContext _dbContext;

        public TestService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedListResponseModel<TestPreviewModel>> GetTestList(TestListRequestModel request)
        {
            var pagesCount = _dbContext.Tests
                .Where(t =>
                    !t.IsDeleted
                    && t.IsPublished);

                throw new NotImplementedException();
        }
    }
}
