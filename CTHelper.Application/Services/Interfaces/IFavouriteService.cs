using CTHelper.Application.Models;
using CTHelper.Application.Models.Favourite;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Models.TestModels;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Services.Interfaces
{
    public interface IFavouriteService
    {
        Task<OperationResult<PaginatedListResponseModel<ProblemListItemModel>>> GetMyFavouriteProblemList(MyFavouriteProblemListRequestModel requestModel);
        Task<OperationResult> AddProblemToFavourite(AddProblemToFavouriteRequestModel requestModel);
        Task<OperationResult> RemoveProblemFromFavourite(RemoveProblemFromFavouriteRequestModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<TestPreviewModel>>> GetMyFavouriteTestList(MyFavouriteTestListRequestModel requestModel);
        Task<OperationResult> AddTestToFavourite(AddTestToFavouriteRequestModel requestModel);
        Task<OperationResult> RemoveTestFromFavourite(RemoveTestFromFavouriteRequestModel requestModel);
    }
}
