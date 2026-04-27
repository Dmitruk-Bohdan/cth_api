using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Assignment;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _dbContext;

        public AssignmentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> AssignTestToStudent(AssignTestToStudentRequestModel requestModel)
        {
            var testExists = await _dbContext.Tests.AnyAsync(t => t.Id == requestModel.TestId && !t.IsDeleted);
            if (!testExists)
            {
                return new OperationResult { ErrorCode = ErrorCodeConstants.TestNotFound, ErrorMessage = "Test not found", HttpStatusCode = HttpStatusCode.NotFound };
            }

            var binding = await _dbContext.TeacherStudents
                .AnyAsync(ts => ts.TeacherId == requestModel.TeacherId && ts.StudentId == requestModel.StudentId && !ts.IsDeleted);
            if (!binding)
            {
                return new OperationResult { ErrorCode = ErrorCodeConstants.BindingNotFound, ErrorMessage = "No binding with this student", HttpStatusCode = HttpStatusCode.NotFound };
            }

            var assignment = new StudentAssignment
            {
                StudentId = requestModel.StudentId,
                TeacherId = requestModel.TeacherId,
                TestId = requestModel.TestId,
                ExpiredAt = requestModel.Deadline ?? DateTimeOffset.MaxValue,
                AttemptsLeft = requestModel.AttemptsAllowed,
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdateAt = DateTimeOffset.UtcNow
            };

            await _dbContext.StudentAssignments.AddAsync(assignment);
            await _dbContext.SaveChangesAsync();
            return new OperationResult();
        }

        public async Task<OperationResult> AssignTestToGroup(AssignTestToGroupRequestModel requestModel)
        {
            var testExists = await _dbContext.Tests.AnyAsync(t => t.Id == requestModel.TestId && !t.IsDeleted);
            if (!testExists)
            {
                return new OperationResult { ErrorCode = ErrorCodeConstants.TestNotFound, ErrorMessage = "Test not found", HttpStatusCode = HttpStatusCode.NotFound };
            }

            var group = await _dbContext.Groups
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == requestModel.GroupId && g.TeacherId == requestModel.TeacherId && !g.IsDeleted);
            if (group == null)
            {
                return new OperationResult { ErrorCode = ErrorCodeConstants.GroupNotFound, ErrorMessage = "Group not found", HttpStatusCode = HttpStatusCode.NotFound };
            }

            var groupAssignment = new GroupAssignment
            {
                GroupId = requestModel.GroupId,
                TeacherId = requestModel.TeacherId,
                TestId = requestModel.TestId,
                ExpiredAt = requestModel.Deadline ?? DateTimeOffset.MaxValue,
                DefaultAttemptsAllowed = requestModel.AttemptsAllowed,
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdateAt = DateTimeOffset.UtcNow
            };

            await _dbContext.GroupAssignments.AddAsync(groupAssignment);
            await _dbContext.SaveChangesAsync();

            foreach (var student in group.Students)
            {
                var studentAssignment = new StudentAssignment
                {
                    StudentId = student.Id,
                    TeacherId = requestModel.TeacherId,
                    TestId = requestModel.TestId,
                    GroupAssignmentId = groupAssignment.Id,
                    ExpiredAt = requestModel.Deadline ?? DateTimeOffset.MaxValue,
                    AttemptsLeft = requestModel.AttemptsAllowed,
                    CreatedAt = DateTimeOffset.UtcNow,
                    LastUpdateAt = DateTimeOffset.UtcNow
                };
                await _dbContext.StudentAssignments.AddAsync(studentAssignment);
            }

            await _dbContext.SaveChangesAsync();
            return new OperationResult();
        }

        public async Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetAssignedToMeList(long userId)
        {
            var assignments = await _dbContext.StudentAssignments
                .Where(sa => sa.StudentId == userId)
                .AsNoTracking()
                .Select(sa => new AssignmentPreviewModel
                {
                    AssignmentId = sa.Id,
                    TeacherName = sa.Teacher.Username,
                    TeacherId = sa.TeacherId,
                    TestName = sa.Test.Title,
                    ExpiredAt = sa.ExpiredAt
                })
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>(
                new PaginatedListResponseModel<AssignmentPreviewModel>
                {
                    Items = assignments,
                    TotalPagesCount = 1,
                    Page = 1,
                    PageSize = assignments.Count,
                    HasPreviousPage = false,
                    HasNextPage = false
                });
        }

        public async Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetAssignedToStudentList(GetAssignedToStudentListModel requestModel)
        {
            var binding = await _dbContext.TeacherStudents
                .AnyAsync(ts => ts.TeacherId == requestModel.TeacherId && ts.StudentId == requestModel.StudentId && !ts.IsDeleted);
            if (!binding)
            {
                return new OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>
                { ErrorCode = ErrorCodeConstants.BindingNotFound, ErrorMessage = "No binding with this student", HttpStatusCode = HttpStatusCode.NotFound };
            }

            var assignments = await _dbContext.StudentAssignments
                .Where(sa => sa.StudentId == requestModel.StudentId && sa.TeacherId == requestModel.TeacherId)
                .AsNoTracking()
                .Select(sa => new AssignmentPreviewModel
                {
                    AssignmentId = sa.Id,
                    TeacherName = sa.Teacher.Username,
                    TeacherId = sa.TeacherId,
                    TestName = sa.Test.Title,
                    ExpiredAt = sa.ExpiredAt
                })
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>(
                new PaginatedListResponseModel<AssignmentPreviewModel>
                {
                    Items = assignments,
                    TotalPagesCount = 1,
                    Page = 1,
                    PageSize = assignments.Count,
                    HasPreviousPage = false,
                    HasNextPage = false
                });
        }

        public async Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetAssignedToGroupList(GetAssignedToGroupListModel requestModel)
        {
            var group = await _dbContext.Groups
                .FirstOrDefaultAsync(g => g.Id == requestModel.GroupId && g.TeacherId == requestModel.TeacherId && !g.IsDeleted);
            if (group == null)
            {
                return new OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>
                { ErrorCode = ErrorCodeConstants.GroupNotFound, ErrorMessage = "Group not found", HttpStatusCode = HttpStatusCode.NotFound };
            }

            var assignments = await _dbContext.GroupAssignments
                .Where(ga => ga.GroupId == requestModel.GroupId && ga.TeacherId == requestModel.TeacherId)
                .AsNoTracking()
                .Select(ga => new AssignmentPreviewModel
                {
                    AssignmentId = ga.Id,
                    TeacherName = ga.Teacher.Username,
                    TeacherId = ga.TeacherId,
                    TestName = ga.Test.Title,
                    ExpiredAt = ga.ExpiredAt
                })
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>(
                new PaginatedListResponseModel<AssignmentPreviewModel>
                {
                    Items = assignments,
                    TotalPagesCount = 1,
                    Page = 1,
                    PageSize = assignments.Count,
                    HasPreviousPage = false,
                    HasNextPage = false
                });
        }

        public async Task<OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>> GetIAssignedList(long userId)
        {
            var studentAssignments = await _dbContext.StudentAssignments
                .Where(sa => sa.TeacherId == userId && sa.GroupAssignmentId == null)
                .AsNoTracking()
                .Select(sa => new AssignmentPreviewModel
                {
                    AssignmentId = sa.Id,
                    TeacherName = sa.Teacher.Username,
                    TeacherId = sa.TeacherId,
                    TestName = sa.Test.Title,
                    ExpiredAt = sa.ExpiredAt
                })
                .ToListAsync();

            var groupAssignments = await _dbContext.GroupAssignments
                .Where(ga => ga.TeacherId == userId)
                .AsNoTracking()
                .Select(ga => new AssignmentPreviewModel
                {
                    AssignmentId = ga.Id,
                    TeacherName = ga.Teacher.Username,
                    TeacherId = ga.TeacherId,
                    TestName = ga.Test.Title,
                    ExpiredAt = ga.ExpiredAt
                })
                .ToListAsync();

            var allAssignments = studentAssignments.Concat(groupAssignments).ToList();

            return new OperationResult<PaginatedListResponseModel<AssignmentPreviewModel>>(
                new PaginatedListResponseModel<AssignmentPreviewModel>
                {
                    Items = allAssignments,
                    TotalPagesCount = 1,
                    Page = 1,
                    PageSize = allAssignments.Count,
                    HasPreviousPage = false,
                    HasNextPage = false
                });
        }

        public async Task<OperationResult> PatchAssignment(PatchAssignmentRequestModel requestModel)
        {
            var studentAssignment = await _dbContext.StudentAssignments
                .FirstOrDefaultAsync(sa => sa.Id == requestModel.AssignmentId && sa.TeacherId == requestModel.TeacherId);
            if (studentAssignment != null)
            {
                if (requestModel.Deadline.HasValue) studentAssignment.ExpiredAt = requestModel.Deadline.Value;
                if (requestModel.Attempts.HasValue) studentAssignment.AttemptsLeft = (short)requestModel.Attempts.Value;
                studentAssignment.LastUpdateAt = DateTimeOffset.UtcNow;
                await _dbContext.SaveChangesAsync();
                return new OperationResult();
            }

            var groupAssignment = await _dbContext.GroupAssignments
                .FirstOrDefaultAsync(ga => ga.Id == requestModel.AssignmentId && ga.TeacherId == requestModel.TeacherId);
            if (groupAssignment != null)
            {
                if (requestModel.Deadline.HasValue)
                {
                    groupAssignment.ExpiredAt = requestModel.Deadline.Value;
                    foreach (var sa in groupAssignment.StudentAssignments)
                    {
                        sa.ExpiredAt = requestModel.Deadline.Value;
                    }
                }
                if (requestModel.Attempts.HasValue)
                {
                    groupAssignment.DefaultAttemptsAllowed = (short)requestModel.Attempts.Value;
                    foreach (var sa in groupAssignment.StudentAssignments)
                    {
                        sa.AttemptsLeft = (short)requestModel.Attempts.Value;
                    }
                }
                groupAssignment.LastUpdateAt = DateTimeOffset.UtcNow;
                await _dbContext.SaveChangesAsync();
                return new OperationResult();
            }

            return new OperationResult { ErrorCode = ErrorCodeConstants.BindingNotFound, ErrorMessage = "Assignment not found", HttpStatusCode = HttpStatusCode.NotFound };
        }

        public async Task<OperationResult> RevokeAssignment(RevokeAssignmentRequestModel requestModel)
        {
            var studentAssignment = await _dbContext.StudentAssignments
                .FirstOrDefaultAsync(sa => sa.Id == requestModel.AssignmentId && sa.TeacherId == requestModel.TeacherId);
            if (studentAssignment != null)
            {
                _dbContext.StudentAssignments.Remove(studentAssignment);
                await _dbContext.SaveChangesAsync();
                return new OperationResult();
            }

            var groupAssignment = await _dbContext.GroupAssignments
                .Include(ga => ga.StudentAssignments)
                .FirstOrDefaultAsync(ga => ga.Id == requestModel.AssignmentId && ga.TeacherId == requestModel.TeacherId);
            if (groupAssignment != null)
            {
                _dbContext.StudentAssignments.RemoveRange(groupAssignment.StudentAssignments);
                _dbContext.GroupAssignments.Remove(groupAssignment);
                await _dbContext.SaveChangesAsync();
                return new OperationResult();
            }

            return new OperationResult { ErrorCode = ErrorCodeConstants.BindingNotFound, ErrorMessage = "Assignment not found", HttpStatusCode = HttpStatusCode.NotFound };
        }

        public async Task<OperationResult<StudentAssignmentDetailsModel>> GetStudentAssignmentDetails(GetAssignmentDetailsModel requestModel)
        {
            var assignment = await _dbContext.StudentAssignments
                .Where(sa => sa.Id == requestModel.AssignmentId)
                .AsNoTracking()
                .Select(sa => new StudentAssignmentDetailsModel
                {
                    AssignmentId = sa.Id,
                    TeacherId = sa.TeacherId,
                    TeacherName = sa.Teacher.Username,
                    TestId = sa.TestId,
                    TestName = sa.Test.Title,
                    ExpiredAt = sa.ExpiredAt,
                    AttemptsLeft = sa.AttemptsLeft ?? 0,
                    CreatedAt = sa.CreatedAt,
                    StudentId = sa.StudentId,
                    StudentName = sa.Student.Username
                })
                .FirstOrDefaultAsync();

            if (assignment == null)
            {
                return new OperationResult<StudentAssignmentDetailsModel>
                { ErrorCode = ErrorCodeConstants.BindingNotFound, ErrorMessage = "Assignment not found", HttpStatusCode = HttpStatusCode.NotFound };
            }

            return new OperationResult<StudentAssignmentDetailsModel>(assignment);
        }

        public async Task<OperationResult<GroupAssignmentDetailsModel>> GetGroupAssignmentDetails(GetAssignmentDetailsModel requestModel)
        {
            var assignment = await _dbContext.GroupAssignments
                .Where(ga => ga.Id == requestModel.AssignmentId)
                .AsNoTracking()
                .Select(ga => new GroupAssignmentDetailsModel
                {
                    AssignmentId = ga.Id,
                    TeacherId = ga.TeacherId,
                    TeacherName = ga.Teacher.Username,
                    TestId = ga.TestId,
                    TestName = ga.Test.Title,
                    ExpiredAt = ga.ExpiredAt,
                    AttemptsLeft = ga.DefaultAttemptsAllowed ?? 0,
                    CreatedAt = ga.CreatedAt,
                    GroupId = ga.GroupId ?? 0,
                    GroupName = ga.Group.Name
                })
                .FirstOrDefaultAsync();

            if (assignment == null)
            {
                return new OperationResult<GroupAssignmentDetailsModel>
                { ErrorCode = ErrorCodeConstants.BindingNotFound, ErrorMessage = "Assignment not found", HttpStatusCode = HttpStatusCode.NotFound };
            }

            return new OperationResult<GroupAssignmentDetailsModel>(assignment);
        }

        public async Task<OperationResult<GroupScoreByAssignmentResponseModel>> GetGroupAssignmentScore(GetGroupAssignmentScoreModel requestModel)
        {
            var groupAssignment = await _dbContext.GroupAssignments
                .Include(ga => ga.StudentAssignments)
                .ThenInclude(sa => sa.Student)
                .ThenInclude(s => s.TestAttempts)
                .FirstOrDefaultAsync(ga => ga.Id == requestModel.AssignmentId && ga.TeacherId == requestModel.TeacherId);

            if (groupAssignment == null)
            {
                return new OperationResult<GroupScoreByAssignmentResponseModel>
                { ErrorCode = ErrorCodeConstants.BindingNotFound, ErrorMessage = "Assignment not found", HttpStatusCode = HttpStatusCode.NotFound };
            }

            var memberScores = groupAssignment.StudentAssignments.Select(sa =>
            {
                var attempts = sa.Student.TestAttempts.Where(ta => ta.TestId == sa.TestId).ToList();
                var bestAttempt = attempts.OrderByDescending(ta => ta.RawScore).FirstOrDefault();
                return new
                {
                    sa.StudentId,
                    StudentName = sa.Student.Username,
                    IsPassed = bestAttempt?.RawScore >= 60,
                    PercentageScore = bestAttempt?.RawScore,
                    AttemptId = bestAttempt?.Id
                };
            }).ToList();

            var avgScore = memberScores.Any() ? (short?)memberScores.Average(m => m.PercentageScore ?? 0) : null;
            var completionRate = groupAssignment.StudentAssignments.Any()
                ? (short?)(memberScores.Count(m => m.IsPassed) * 100 / groupAssignment.StudentAssignments.Count)
                : null;

            var result = new GroupScoreByAssignmentResponseModel
            {
                AveragePercentageScore = avgScore,
                PercentageOfСompletion = completionRate
            };

            return new OperationResult<GroupScoreByAssignmentResponseModel>(result);
        }

        public async Task<OperationResult<StudentScoreByAssignmentResponseModel>> GetStudentAssignmentScore(GetStudentAssignmentScoreModel requestModel)
        {
            var studentAssignment = await _dbContext.StudentAssignments
                .Include(sa => sa.Student)
                .ThenInclude(s => s.TestAttempts)
                .FirstOrDefaultAsync(sa => sa.Id == requestModel.AssignmentId && sa.TeacherId == requestModel.TeacherId);

            if (studentAssignment == null)
            {
                return new OperationResult<StudentScoreByAssignmentResponseModel>
                { ErrorCode = ErrorCodeConstants.BindingNotFound, ErrorMessage = "Assignment not found", HttpStatusCode = HttpStatusCode.NotFound };
            }

            var attempts = studentAssignment.Student.TestAttempts
                .Where(ta => ta.TestId == studentAssignment.TestId)
                .Select(ta => new StudentScoreByAssignmentItemResponseDto
                {
                    PercentageScore = ta.RawScore,
                    AttemptId = ta.Id,
                    Duration = (short)ta.Duration
                })
                .ToList();

            var result = new StudentScoreByAssignmentResponseModel
            {
                Attempts = attempts,
                AttemptsLeft = studentAssignment.AttemptsLeft > 0,
                AveragePercentageScore = attempts.Any() ? (short?)attempts.Average(a => a.PercentageScore ?? 0) : null,
                BestPercentageScore = attempts.Any() ? attempts.Max(a => a.PercentageScore) : null
            };

            return new OperationResult<StudentScoreByAssignmentResponseModel>(result);
        }
    }
}
