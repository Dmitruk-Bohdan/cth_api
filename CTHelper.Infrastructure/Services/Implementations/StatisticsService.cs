using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Statistics;
using CTHelper.Application.Services.Interfaces;
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
            var dateFrom = requestModel.DateFrom ?? DateTimeOffset.MinValue;
            var dateTo = requestModel.DateTo ?? DateTimeOffset.MaxValue;

            var answers = await _dbContext.UserAnswers
                .Where(ua => ua.TestAttempt.StudentId == requestModel.UserId)
                .Where(ua => ua.TestAttempt.CreatedAt >= dateFrom && ua.TestAttempt.CreatedAt <= dateTo)
                .Where(ua => ua.TestAttempt.Test.SubjectId == requestModel.SubjectId)
                .AsNoTracking()
                .Select(ua => new
                {
                    ua.IsCorrect,
                    TopicId = ua.ProblemVersion.Problem.TopicId,
                    TopicName = ua.ProblemVersion.Problem.Topic.Name,
                    Difficulty = ua.ProblemVersion.Difficulty
                })
                .ToListAsync();

            var totalAttempts = answers.Count;
            var correctAttempts = answers.Count(a => a.IsCorrect);
            var commonRate = totalAttempts > 0 ? (int)((double)correctAttempts / totalAttempts * 100) : 0;

            var topicGroups = answers
                .GroupBy(a => new { a.TopicId, a.TopicName })
                .Select(g => new TopicStatisticsModel
                {
                    TopicId = g.Key.TopicId,
                    TopicName = g.Key.TopicName,
                    AverageSuccessRate = g.Any() ? (int)((double)g.Count(x => x.IsCorrect) / g.Count() * 100) : 0,
                    SuccessRateByDifficultList = g.GroupBy(x => x.Difficulty)
                        .Select(dg => new SuccessByDifficultModel
                        {
                            Difficult = dg.Key,
                            SuccessRate = dg.Any() ? (int)((double)dg.Count(x => x.IsCorrect) / dg.Count() * 100) : 0
                        }).ToList()
                }).ToList();

            var allTopicIds = topicGroups.Select(t => t.TopicId).ToList();
            var pendingTopics = await _dbContext.Topics
                .Where(t => t.Section.SubjectId == requestModel.SubjectId && !allTopicIds.Contains(t.Id))
                .AsNoTracking()
                .Select(t => new TopicModel { TopicId = t.Id, TopicName = t.Name })
                .ToListAsync();

            var avgRate = topicGroups.Any() ? topicGroups.Average(t => t.AverageSuccessRate) : 0;
            var topicsToReview = topicGroups
                .Where(t => t.AverageSuccessRate < avgRate)
                .Select(t => new TopicModel { TopicId = t.TopicId, TopicName = t.TopicName })
                .ToList();

            var result = new StudentStatisticsModel
            {
                FromDate = dateFrom,
                ToDate = dateTo,
                CommonRate = commonRate,
                TotalAttempts = totalAttempts,
                CorrectAttempts = correctAttempts,
                StatisticsByTopicList = topicGroups,
                PendingTopicList = pendingTopics,
                TopicToReviewList = topicsToReview
            };

            return new OperationResult<StudentStatisticsModel>(result);
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

            var dateFrom = requestModel.DateFrom ?? DateTimeOffset.MinValue;
            var dateTo = requestModel.DateTo ?? DateTimeOffset.MaxValue;

            var answers = await _dbContext.UserAnswers
                .Where(ua => ua.TestAttempt.StudentId == requestModel.StudentId)
                .Where(ua => ua.TestAttempt.CreatedAt >= dateFrom && ua.TestAttempt.CreatedAt <= dateTo)
                .Where(ua => ua.TestAttempt.Test.SubjectId == requestModel.SubjectId)
                .AsNoTracking()
                .Select(ua => new
                {
                    ua.IsCorrect,
                    TopicId = ua.ProblemVersion.Problem.TopicId,
                    TopicName = ua.ProblemVersion.Problem.Topic.Name,
                    Difficulty = ua.ProblemVersion.Difficulty
                })
                .ToListAsync();

            var totalAttempts = answers.Count;
            var correctAttempts = answers.Count(a => a.IsCorrect);
            var commonRate = totalAttempts > 0 ? (int)((double)correctAttempts / totalAttempts * 100) : 0;

            var topicGroups = answers
                .GroupBy(a => new { a.TopicId, a.TopicName })
                .Select(g => new TopicStatisticsModel
                {
                    TopicId = g.Key.TopicId,
                    TopicName = g.Key.TopicName,
                    AverageSuccessRate = g.Any() ? (int)((double)g.Count(x => x.IsCorrect) / g.Count() * 100) : 0,
                    SuccessRateByDifficultList = g.GroupBy(x => x.Difficulty)
                        .Select(dg => new SuccessByDifficultModel
                        {
                            Difficult = dg.Key,
                            SuccessRate = dg.Any() ? (int)((double)dg.Count(x => x.IsCorrect) / dg.Count() * 100) : 0
                        }).ToList()
                }).ToList();

            var allTopicIds = topicGroups.Select(t => t.TopicId).ToList();
            var pendingTopics = await _dbContext.Topics
                .Where(t => t.Section.SubjectId == requestModel.SubjectId && !allTopicIds.Contains(t.Id))
                .AsNoTracking()
                .Select(t => new TopicModel { TopicId = t.Id, TopicName = t.Name })
                .ToListAsync();

            var avgRate = topicGroups.Any() ? topicGroups.Average(t => t.AverageSuccessRate) : 0;
            var topicsToReview = topicGroups
                .Where(t => t.AverageSuccessRate < avgRate)
                .Select(t => new TopicModel { TopicId = t.TopicId, TopicName = t.TopicName })
                .ToList();

            var result = new StudentStatisticsModel
            {
                FromDate = dateFrom,
                ToDate = dateTo,
                CommonRate = commonRate,
                TotalAttempts = totalAttempts,
                CorrectAttempts = correctAttempts,
                StatisticsByTopicList = topicGroups,
                PendingTopicList = pendingTopics,
                TopicToReviewList = topicsToReview
            };

            return new OperationResult<StudentStatisticsModel>(result);
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

            var dateFrom = requestModel.DateFrom ?? DateTimeOffset.MinValue;
            var dateTo = requestModel.DateTo ?? DateTimeOffset.MaxValue;

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

            var studentNames = await _dbContext.Users
                .Where(u => studentIds.Contains(u.Id))
                .AsNoTracking()
                .ToDictionaryAsync(u => u.Id, u => u.Username);

            var answers = await _dbContext.UserAnswers
                .Where(ua => studentIds.Contains(ua.TestAttempt.StudentId))
                .Where(ua => ua.TestAttempt.CreatedAt >= dateFrom && ua.TestAttempt.CreatedAt <= dateTo)
                .Where(ua => ua.TestAttempt.Test.SubjectId == requestModel.SubjectId)
                .AsNoTracking()
                .Select(ua => new
                {
                    ua.IsCorrect,
                    StudentId = ua.TestAttempt.StudentId,
                    TopicId = ua.ProblemVersion.Problem.TopicId,
                    TopicName = ua.ProblemVersion.Problem.Topic.Name,
                    Difficulty = ua.ProblemVersion.Difficulty
                })
                .ToListAsync();

            var studentStats = studentIds.Select(sid => new
            {
                StudentId = sid,
                Answers = answers.Where(a => a.StudentId == sid).ToList()
            }).Select(s => new
            {
                s.StudentId,
                Total = s.Answers.Count,
                Correct = s.Answers.Count(a => a.IsCorrect),
                Rate = s.Answers.Any() ? (int)((double)s.Answers.Count(a => a.IsCorrect) / s.Answers.Count * 100) : 0
            }).ToList();

            var members = studentStats
                .Select(s => new GroupMemberStatisticItem
                {
                    StudentId = s.StudentId,
                    StudentName = studentNames.GetValueOrDefault(s.StudentId) ?? string.Empty,
                    StudentRate = s.Rate,
                    StudentGroupRating = 0
                }).ToList();

            var ordered = members.OrderByDescending(m => m.StudentRate).ToList();
            for (int i = 0; i < ordered.Count; i++)
            {
                ordered[i].StudentGroupRating = i + 1;
            }

            var topicGroups = answers
                .GroupBy(a => new { a.TopicId, a.TopicName })
                .Select(g => new TopicStatisticsModel
                {
                    TopicId = g.Key.TopicId,
                    TopicName = g.Key.TopicName,
                    AverageSuccessRate = g.Any() ? (int)((double)g.Count(x => x.IsCorrect) / g.Count() * 100) : 0,
                    SuccessRateByDifficultList = g.GroupBy(x => x.Difficulty)
                        .Select(dg => new SuccessByDifficultModel
                        {
                            Difficult = dg.Key,
                            SuccessRate = dg.Any() ? (int)((double)dg.Count(x => x.IsCorrect) / dg.Count() * 100) : 0
                        }).ToList()
                }).ToList();

            var allTopicIds = topicGroups.Select(t => t.TopicId).ToList();
            var pendingTopics = await _dbContext.Topics
                .Where(t => t.Section.SubjectId == requestModel.SubjectId && !allTopicIds.Contains(t.Id))
                .AsNoTracking()
                .Select(t => new TopicModel { TopicId = t.Id, TopicName = t.Name })
                .ToListAsync();

            var avgRate = topicGroups.Any() ? topicGroups.Average(t => t.AverageSuccessRate) : 0;
            var topicsToReview = topicGroups
                .Where(t => t.AverageSuccessRate < avgRate)
                .Select(t => new TopicModel { TopicId = t.TopicId, TopicName = t.TopicName })
                .ToList();

            var result = new GroupStatisticsModel
            {
                FromDate = dateFrom,
                ToDate = dateTo,
                Members = members,
                StatisticsByTopicList = topicGroups,
                PendingTopicList = pendingTopics,
                TopicToReviewList = topicsToReview
            };

            return new OperationResult<GroupStatisticsModel>(result);
        }
    }
}
