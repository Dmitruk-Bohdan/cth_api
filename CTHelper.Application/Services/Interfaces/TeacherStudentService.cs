using CTHelper.Application.Models;
using CTHelper.Application.Models.TeacherStudent;
using CTHelper.Application.Models.User;

namespace CTHelper.Application.Services.Interfaces
{
    public interface ITeacherStudentService
    {
        Task<OperationResult> AcceptStudentByInvitationCode(long teacherId, long bindingRequestId);
        Task<OperationResult> BlockStudent(long teacherId, long studentId);
        Task<OperationResult<CreateInvitationCodeResponseModel>> CreateInvitationCodeAsync(long teacherId, short? usesCount, DateTimeOffset? expiredAt);
        Task<OperationResult<List<UserProfilePreviewWithAvatarUrlModel>>> GetBlockedStudentList(long teacherId);
        Task<OperationResult<UserProfileResponseModel>> GetMyStudentInfoById(long teacherId, long studentId);
        Task<OperationResult<List<UserProfilePreviewWithAvatarUrlModel>>> GetMyStudentsList(long teacherId);
        Task<OperationResult<UserProfileResponseModel>> GetMyTeacherInfoById(long teacherId, long studentId);
        Task<OperationResult<List<UserProfilePreviewWithAvatarUrlModel>>> GetMyTeachersList(long studentId);
        Task<OperationResult> RemoveBindingWithStudent(long teacherId, long bindingId);
        Task<OperationResult> RemoveBindingWithTeacher(long studentId, long bindingId);
        Task<OperationResult> RequestBindingWithTeacherByCode(long studentId, string code);
        Task<OperationResult> UnblockStudent(long teacherId, long bindingId);
    }
}
