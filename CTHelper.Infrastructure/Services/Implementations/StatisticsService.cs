using CTHelper.Application.Models;
using CTHelper.Application.Models.Statistics;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Presentation.Controllers;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class StatisticsService : IStatisticsService
    {
        public Task<OperationResult<GroupStatisticsModel>> GetGroupStatisticsBySubject(GroupStatisticsBySubjectRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<StudentStatisticsModel>> GetMyStatisticsBySubject(MyStatisticsBySubjectRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<StudentStatisticsModel>> GetStudentStatisticsBySubject(StudentStatisticsBySubjectRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
