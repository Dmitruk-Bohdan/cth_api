using CTHelper.Application.Models;
using CTHelper.Application.Models.Favourite;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Models.Test;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class FavouriteService : IFavouriteService
    {
        public Task<OperationResult> AddProblemToFavourite(AddProblemToFavouriteRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> AddTestToFavourite(AddTestToFavouriteRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<ProblemPreviewModel>>> GetMyFavouriteProblemList(MyFavouriteProblemListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<TestPreviewModel>>> GetMyFavouriteTestList(MyFavouriteTestListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> RemoveProblemFromFavourite(RemoveProblemFromFavouriteRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> RemoveTestFrom(RemoveTestFromFavouriteRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
