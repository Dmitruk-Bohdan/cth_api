using CTHelper.Application.Models;
using CTHelper.Application.Models.Assignment;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        public Task<OperationResult> AssignTestToGroup(AssignTestToGroupRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> AssignTestToStudent(AssignTestToStudentRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetAssignedToGroupList(GetAssignedToGroupListModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetAssignedToMeList(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetAssignedToStudentList(GetAssignedToStudentListModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<GroupAssignmentDetailsModel>> GetGroupAssignmentDetails(GetAssignmentDetailsModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<GroupScoreByAssignmentResponseModel>> GetGroupAssignmentScore(GetGroupAssignmentScoreModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetIAssignedList(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<StudentAssignmentDetailsModel>> GetStudentAssignmentDetails(GetAssignmentDetailsModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<StudentScoreByAssignmentResponseModel>> GetStudentAssignmentScore(GetStudentAssignmentScoreModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> PatchAssignment(PatchAssignmentRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> RevokeAssignment(RevokeAssignmentRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
