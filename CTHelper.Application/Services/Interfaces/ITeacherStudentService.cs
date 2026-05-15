using CTHelper.Application.Models;
using CTHelper.Application.Models.TeacherStudent;
using CTHelper.Application.Models.UserModels;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Services.Interfaces
{
    public interface ITeacherStudentService
    {
        Task<OperationResult> AcceptStudentByInvitationCode(long teacherId, long bindingRequestId);
        Task<OperationResult> BlockStudent(long teacherId, long studentId);
        Task<OperationResult<CreateInvitationCodeResponseModel>> CreateInvitationCodeAsync(long teacherId, short? usesCount, DateTimeOffset? expiredAt);
        Task<OperationResult<PaginatedListResponseModel<UserProfilePreviewModel>>> GetBlockedStudentList(MyBlockedStudentListRequestModel requestModel);
        Task<OperationResult<UserProfileResponseModel>> GetMyStudentInfoById(long teacherId, long studentId);
        Task<OperationResult<PaginatedListResponseModel<UserProfilePreviewModel>>> GetMyStudentsList(MyStudentsListRequestModel requestModel);
        Task<OperationResult<UserProfileResponseModel>> GetMyTeacherInfoById(long teacherId, long studentId);
        Task<OperationResult<PaginatedListResponseModel<UserProfilePreviewModel>>> GetMyTeachersList(MyTeachersListRequestModel requestModel);
        Task<OperationResult> RemoveBindingWithStudent(long teacherId, long studentId);
        Task<OperationResult> RemoveBindingWithTeacher(long studentId, long teacherId);
        Task<OperationResult> RequestBindingWithTeacherByCode(long studentId, string code);
        Task<OperationResult> UnblockStudent(long teacherId, long studentId);
        Task<OperationResult<PaginatedListResponseModel<BindingRequestResponseModel>>> GetPendingBindingRequests(MyPendingBindingRequestsRequestModel requestModel);
    }
}
