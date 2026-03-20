using CTHelper.Application.Models;
using CTHelper.Application.Models.TeacherStudent;

namespace CTHelper.Application.Services.Interfaces
{
    public interface ITeacherStudentService
    {
        Task<OperationResult<CreateInvitationCodeResponseModel>> CreateInvitationCodeAsync(long teacherId, short? usesCount, DateTimeOffset? expiredAt);
    }
}
