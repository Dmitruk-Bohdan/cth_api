using CTHelper.Application.Models;
using CTHelper.Application.Models.Statistics;
using CTHelper.Presentation.Controllers;

namespace CTHelper.Application.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<OperationResult<GroupStatisticsModel>> GetGroupStatisticsBySubject(GroupStatisticsBySubjectRequestModel requestModel);
        Task<OperationResult<StudentStatisticsModel>> GetMyStatisticsBySubject(MyStatisticsBySubjectRequestModel requestModel);
        Task<OperationResult<StudentStatisticsModel>> GetStudentStatisticsBySubject(StudentStatisticsBySubjectRequestModel requestModel);
    }
}
