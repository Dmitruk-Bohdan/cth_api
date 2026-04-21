using CTHelper.Application.Models;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Entities;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Infrastructure.Services.Implementations
{
    class ProblemService : IProblemService
    {
        public Task<OperationResult> CreateProblem(CreateProblemRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteProblem(DeleteProblemRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<ProblemDetailsModel>> GetProblemDetailsAsync(ProblemDetailsRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<ProblemListItemModel>>> GetProblemListAsync(ProblemListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Problem>> UpdateProblem(UpdateProblemRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
