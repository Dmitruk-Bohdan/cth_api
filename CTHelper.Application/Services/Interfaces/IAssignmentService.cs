using CTHelper.Application.Models;
using CTHelper.Application.Models.Assignment;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<OperationResult> AssignTestToGroup(AssignTestToGroupRequestModel requestModel);
        Task<OperationResult> AssignTestToStudent(AssignTestToStudentRequestModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetAssignedToMeList(long userId);
        Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetAssignedToStudentList(GetAssignedToStudentListModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetAssignedToGroupList(GetAssignedToGroupListModel requestModel);
        Task<OperationResult<GroupScoreByAssignmentResponseModel>> GetGroupAssignmentScore(GetGroupAssignmentScoreModel requestModel);
        Task<OperationResult<StudentScoreByAssignmentResponseModel>> GetStudentAssignmentScore(GetStudentAssignmentScoreModel requestModel);
        Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetIAssignedList(long userId);
        Task<OperationResult> PatchAssignment(PatchAssignmentRequestModel requestModel);
        Task<OperationResult> RevokeAssignment(RevokeAssignmentRequestModel requestModel);
        Task<OperationResult<StudentAssignmentDetailsModel>> GetStudentAssignmentDetails(GetAssignmentDetailsModel requestModel);
        Task<OperationResult<GroupAssignmentDetailsModel>> GetGroupAssignmentDetails(GetAssignmentDetailsModel requestModel);
    }
}
