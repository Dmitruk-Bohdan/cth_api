using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Statistics;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Extensions;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class StatisticsService : IStatisticsService
    {
        private readonly AppDbContext _dbContext;

        public StatisticsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<StudentStatisticsModel>> GetMyStatisticsBySubject(MyStatisticsBySubjectRequestModel requestModel)
        {
            var statistics = await BuildStudentStatistics(requestModel.UserId, requestModel.SubjectId, requestModel.DateFrom, requestModel.DateTo);
            return new OperationResult<StudentStatisticsModel>(statistics);
        }

        public async Task<OperationResult<StudentStatisticsModel>> GetStudentStatisticsBySubject(StudentStatisticsBySubjectRequestModel requestModel)
        {
            var hasBinding = await _dbContext.TeacherStudents
                .Where(ts => ts.TeacherId == requestModel.UserId && ts.StudentId == requestModel.StudentId && !ts.IsDeleted)
                .AnyAsync();

            if (!hasBinding)
            {
                return new OperationResult<StudentStatisticsModel>
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You do not have access to this student's statistics",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var statistics = await BuildStudentStatistics(requestModel.StudentId, requestModel.SubjectId, requestModel.DateFrom, requestModel.DateTo);
            return new OperationResult<StudentStatisticsModel>(statistics);
        }

        public async Task<OperationResult<GroupStatisticsModel>> GetGroupStatisticsBySubject(GroupStatisticsBySubjectRequestModel requestModel)
        {
            var groupBelongsToTeacher = await _dbContext.Groups
                .Where(g => g.Id == requestModel.GroupId && g.TeacherId == requestModel.UserId)
                .AnyAsync();

            if (!groupBelongsToTeacher)
            {
                return new OperationResult<GroupStatisticsModel>
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You do not have access to this group's statistics",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var studentIds = await _dbContext.Groups
                .Where(g => g.Id == requestModel.GroupId)
                .SelectMany(g => g.Students.Select(s => s.Id))
                .ToListAsync();

            if (!studentIds.Any())
            {
                return new OperationResult<GroupStatisticsModel>
                {
                    ErrorCode = ErrorCodeConstants.StudentNotBelongToGroup,
                    ErrorMessage = "Group has no students",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var answersWithNames = await _dbContext.UserAnswers
                .Where(ua => studentIds.Contains(ua.TestAttempt.StudentId))
                .ApplyDateFilter(requestModel.DateFrom, requestModel.DateTo)
                .Where(ua => ua.TestAttempt.Test.SubjectId == requestModel.SubjectId)
                .AsNoTracking()
                .Select(ua => new
                {
                    ua.IsCorrect,
                    StudentId = ua.TestAttempt.StudentId,
                    StudentName = ua.TestAttempt.Student.Username,
                    TopicId = ua.ProblemVersion.Problem.TopicId,
                    TopicName = ua.ProblemVersion.Problem.Topic.Name,
                    Difficulty = ua.ProblemVersion.Difficulty
                })
                .ToListAsync();

            var studentStats = answersWithNames
                .GroupBy(a => new { a.StudentId, a.StudentName })
                .Select(g => new
                {
                    g.Key.StudentId,
                    g.Key.StudentName,
                    Total = g.Count(),
                    Correct = g.Count(a => a.IsCorrect),
                    Rate = g.Any() ? (int)((double)g.Count(a => a.IsCorrect) / g.Count() * 100) : 0
                })
                .OrderByDescending(s => s.Rate)
                .ToList();

            var members = studentStats
                .Select((s, index) => new GroupMemberStatisticItem
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    StudentRate = s.Rate,
                    StudentGroupRating = index + 1
                })
                .ToList();

            var topicGroups = BuildTopicStatistics(answersWithNames);

            var allTopicIds = topicGroups.Select(t => t.TopicId).ToList();
            var pendingTopics = await _dbContext.Topics
                .Where(t => t.Section.SubjectId == requestModel.SubjectId && !allTopicIds.Contains(t.Id))
                .AsNoTracking()
                .Select(t => new TopicModel { TopicId = t.Id, TopicName = t.Name })
                .ToListAsync();

            var medianRate = CalculateMedian(topicGroups.Select(t => t.AverageSuccessRate).ToList());
            var topicsToReview = topicGroups
                .Where(t => t.AverageSuccessRate < medianRate)
                .Select(t => new TopicModel { TopicId = t.TopicId, TopicName = t.TopicName })
                .ToList();

            var result = new GroupStatisticsModel
            {
                FromDate = requestModel.DateFrom,
                ToDate = requestModel.DateTo,
                Members = members,
                StatisticsByTopicList = topicGroups,
                PendingTopicList = pendingTopics,
                TopicToReviewList = topicsToReview
            };

            return new OperationResult<GroupStatisticsModel>(result);
        }

        private async Task<StudentStatisticsModel> BuildStudentStatistics(
            long studentId,
            long subjectId,
            DateTimeOffset? dateFrom,
            DateTimeOffset? dateTo)
        {
            var answers = await _dbContext.UserAnswers
                .Where(ua => ua.TestAttempt.StudentId == studentId)
                .ApplyDateFilter(dateFrom, dateTo)
                .Where(ua => ua.TestAttempt.Test.SubjectId == subjectId)
                .AsNoTracking()
                .Select(ua => new
                {
                    ua.IsCorrect,
                    TopicId = ua.ProblemVersion.Problem.TopicId,
                    TopicName = ua.ProblemVersion.Problem.Topic.Name,
                    Difficulty = ua.ProblemVersion.Difficulty
                })
                .ToListAsync();

            var attempts = await _dbContext.TestAttempts
                .Where(ta => ta.StudentId == studentId)
                .ApplyDateFilter(dateFrom, dateTo)
                .Where(ta => ta.Test.SubjectId == subjectId)
                .AsNoTracking()
                .CountAsync();

            var totalAnswers = answers.Count;
            var correctAnswers = answers.Count(a => a.IsCorrect);
            var commonRate = totalAnswers > 0 ? (int)((double)correctAnswers / totalAnswers * 100) : 0;

            var topicGroups = BuildTopicStatistics(answers);

            var topicRates = topicGroups.Select(t => t.AverageSuccessRate).OrderBy(r => r).ToList();
            var medianRate = CalculateMedian(topicRates);

            var allTopicIds = topicGroups.Select(t => t.TopicId).ToList();
            var pendingTopics = await _dbContext.Topics
                .Where(t => t.Section.SubjectId == subjectId && !allTopicIds.Contains(t.Id))
                .AsNoTracking()
                .Select(t => new TopicModel { TopicId = t.Id, TopicName = t.Name })
                .ToListAsync();

            var topicsToReview = topicGroups
                .Where(t => t.AverageSuccessRate < medianRate)
                .Select(t => new TopicModel { TopicId = t.TopicId, TopicName = t.TopicName })
                .ToList();

            var result = new StudentStatisticsModel
            {
                FromDate = dateFrom,
                ToDate = dateTo,
                CommonRate = commonRate,
                MedianRate = medianRate,
                TotalAnswers = totalAnswers,
                CorrectAnswers = correctAnswers,
                TotalAttempts = attempts,
                StatisticsByTopicList = topicGroups,
                PendingTopicList = pendingTopics,
                TopicToReviewList = topicsToReview
            };

            return result;
        }

        private List<TopicStatisticsModel> BuildTopicStatistics(IEnumerable<dynamic> answers)
        {
            return answers
                .GroupBy(a => new { a.TopicId, a.TopicName })
                .Select(g =>
                {
                    var correctCount = g.Count(x => x.IsCorrect);
                    var totalCount = g.Count();
                    var avgRate = totalCount > 0 ? (int)((double)correctCount / totalCount * 100) : 0;

                    var difficultyRates = g.GroupBy(x => (int)x.Difficulty)
                        .Select(dg => new { Rate = dg.Any() ? (double)dg.Count(x => x.IsCorrect) / dg.Count() * 100 : 0 })
                        .OrderBy(r => r.Rate)
                        .Select(r => r.Rate)
                        .ToList();

                    var medianRate = CalculateMedian(difficultyRates);

                    return new TopicStatisticsModel
                    {
                        TopicId = g.Key.TopicId,
                        TopicName = g.Key.TopicName,
                        AverageSuccessRate = avgRate,
                        MedianSuccessRate = medianRate,
                        SuccessRateByDifficultList = g.GroupBy(x => (int)x.Difficulty)
                            .Select(dg =>
                            {
                                var correctInDifficulty = dg.Count(x => x.IsCorrect);
                                var totalInDifficulty = dg.Count();
                                var diffSuccessRate = totalInDifficulty > 0 ? (int)((double)correctInDifficulty / totalInDifficulty * 100) : 0;

                                return new SuccessByDifficultModel
                                {
                                    Difficult = dg.Key,
                                    SuccessRate = diffSuccessRate,
                                    MedianSuccessRate = diffSuccessRate
                                };
                            }).ToList()
                    };
                }).ToList();
        }

        private int CalculateMedian(List<int> sortedValues)
        {
            if (!sortedValues.Any())
                return 0;

            var count = sortedValues.Count;
            if (count % 2 == 0)
                return (int)((sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2.0);
            else
                return sortedValues[count / 2];
        }

        private int CalculateMedian(List<double> sortedValues)
        {
            if (!sortedValues.Any())
                return 0;

            var count = sortedValues.Count;
            if (count % 2 == 0)
                return (int)((sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2.0);
            else
                return (int)sortedValues[count / 2];
        }
    }
}