using CTHelper.Application.Models;
using CTHelper.Application.Models.Problem;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Services.Interfaces
{
    public interface IProblemService
    {
        Task<OperationResult<ProblemDetailsModel>> GetProblemDetailsAsync(ProblemDetailsRequestModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<ProblemPreviewModel>>> GetProblemListAsync(ProblemListRequestModel requestModel);
    }
}
