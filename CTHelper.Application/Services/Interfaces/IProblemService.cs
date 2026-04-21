using CTHelper.Application.Models;
using CTHelper.Application.Models.Problem;
using CTHelper.Domain.Entities;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Services.Interfaces
{
    public interface IProblemService
    {
        Task<OperationResult> CreateProblem(CreateProblemRequestModel requestModel);
        Task<OperationResult> DeleteProblem(DeleteProblemRequestModel requestModel);
        Task<OperationResult<ProblemDetailsModel>> GetProblemDetailsAsync(ProblemDetailsRequestModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<ProblemListItemModel>>> GetProblemListAsync(ProblemListRequestModel requestModel);
        Task<OperationResult<Problem>> UpdateProblem(UpdateProblemRequestModel requestModel);
    }
}
