using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Assignment;
using CTHelper.Application.Models.AssignmentModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

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
            var test = await _dbContext.Tests
                   .AsNoTracking()
                   .FirstOrDefaultAsync(t => t.Id == requestModel.TestId && !t.IsDeleted);

            if (test == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Test not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (!test.IsPublished)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.TestNotPublished,
                    ErrorMessage = "Cannot assign unpublished test",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }


            var binding = await _dbContext.TeacherStudents
                .AnyAsync(ts => ts.TeacherId == requestModel.TeacherId && ts.StudentId == requestModel.StudentId && !ts.IsDeleted);
            if (!binding)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.BindingNotFound,
                    ErrorMessage = "No binding with this student",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var existingAssignment = await _dbContext.StudentAssignments
                .AnyAsync(sa =>
                    sa.StudentId == requestModel.StudentId
                    && sa.TeacherId == requestModel.TeacherId
                    && sa.TestId == requestModel.TestId
                    && !sa.IsDeleted);
            if (existingAssignment)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AssignmentAlreadyExists,
                    ErrorMessage = "This test is already assigned to this student",
                    HttpStatusCode = HttpStatusCode.Conflict
                };
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
            var test = await _dbContext.Tests
                    .AsNoTracking()
                   .FirstOrDefaultAsync(t => t.Id == requestModel.TestId && !t.IsDeleted);

            if (test == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Test not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (!test.IsPublished)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.TestNotPublished,
                    ErrorMessage = "Cannot assign unpublished test",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var group = await _dbContext.Groups
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == requestModel.GroupId && g.TeacherId == requestModel.TeacherId && !g.IsDeleted);
            if (group == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.GroupNotFound,
                    ErrorMessage = "Group not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var existingGroupAssignment = await _dbContext.GroupAssignments
                .AnyAsync(ga =>
                    ga.GroupId == requestModel.GroupId
                    && ga.TeacherId == requestModel.TeacherId
                    && ga.TestId == requestModel.TestId
                    && !ga.IsDeleted);
            if (existingGroupAssignment)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AssignmentAlreadyExists,
                    ErrorMessage = "This test is already assigned to this group",
                    HttpStatusCode = HttpStatusCode.Conflict
                };
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

            var studentAssignments = group.Students.Select(student => new StudentAssignment
            {
                StudentId = student.Id,
                TeacherId = requestModel.TeacherId,
                TestId = requestModel.TestId,
                GroupAssignmentId = groupAssignment.Id,
                ExpiredAt = requestModel.Deadline ?? DateTimeOffset.MaxValue,
                AttemptsLeft = requestModel.AttemptsAllowed,
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdateAt = DateTimeOffset.UtcNow
            });

            await _dbContext.StudentAssignments.AddRangeAsync(studentAssignments);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult<PaginatedListResponseModel<StudentAssignmentPreviewModel>>> GetAssignedToMeList(GetAssignedToMeRequestModel requestModel)
        {
            var assignmentsQuery = _dbContext.StudentAssignments
                .Where(sa => sa.StudentId == requestModel.UserId && !sa.IsDeleted)
                .AsNoTracking();

            var totalCount = await assignmentsQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)totalCount / requestModel.PageSize);

            var assignments = await assignmentsQuery
                .OrderBy(sa => sa.ExpiredAt)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .Select(sa => new StudentAssignmentPreviewModel
                {
                    AssignmentId = sa.Id,
                    TeacherName = sa.Teacher.Username,
                    TeacherId = sa.TeacherId,
                    TestName = sa.Test.Title,
                    ExpiredAt = sa.ExpiredAt
                })
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<StudentAssignmentPreviewModel>>(
                new PaginatedListResponseModel<StudentAssignmentPreviewModel>
                {
                    Items = assignments,
                    TotalPagesCount = pagesCount,
                    Page = requestModel.PageNumber,
                    PageSize = requestModel.PageSize,
                    HasPreviousPage = requestModel.PageNumber > 1,
                    HasNextPage = requestModel.PageNumber < pagesCount
                });
        }

        public async Task<OperationResult<PaginatedListResponseModel<StudentAssignmentPreviewModel>>> GetAssignedToStudentList(GetAssignedToStudentListModel requestModel)
        {
            var binding = await _dbContext.TeacherStudents
                .AnyAsync(ts => ts.TeacherId == requestModel.TeacherId && ts.StudentId == requestModel.StudentId && !ts.IsDeleted);
            if (!binding)
            {
                return new OperationResult<PaginatedListResponseModel<StudentAssignmentPreviewModel>>
                {
                    ErrorCode = ErrorCodeConstants.BindingNotFound,
                    ErrorMessage = "No binding with this student",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var assignmentsQuery = _dbContext.StudentAssignments
                .Where(sa => sa.StudentId == requestModel.StudentId && sa.TeacherId == requestModel.TeacherId && !sa.IsDeleted)
                .AsNoTracking();

            var totalCount = await assignmentsQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)totalCount / requestModel.PageSize);

            var assignments = await assignmentsQuery
                .OrderBy(sa => sa.ExpiredAt)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .Select(sa => new StudentAssignmentPreviewModel
                {
                    AssignmentId = sa.Id,
                    TeacherName = sa.Teacher.Username,
                    TeacherId = sa.TeacherId,
                    TestName = sa.Test.Title,
                    ExpiredAt = sa.ExpiredAt
                })
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<StudentAssignmentPreviewModel>>(
                new PaginatedListResponseModel<StudentAssignmentPreviewModel>
                {
                    Items = assignments,
                    TotalPagesCount = pagesCount,
                    Page = requestModel.PageNumber,
                    PageSize = requestModel.PageSize,
                    HasPreviousPage = requestModel.PageNumber > 1,
                    HasNextPage = requestModel.PageNumber < pagesCount
                });
        }

        public async Task<OperationResult<PaginatedListResponseModel<StudentAssignmentPreviewModel>>> GetAssignedToGroupList(GetAssignedToGroupListModel requestModel)
        {
            var group = await _dbContext.Groups
                .FirstOrDefaultAsync(g => g.Id == requestModel.GroupId && g.TeacherId == requestModel.TeacherId && !g.IsDeleted);
            if (group == null)
            {
                return new OperationResult<PaginatedListResponseModel<StudentAssignmentPreviewModel>>
                {
                    ErrorCode = ErrorCodeConstants.GroupNotFound,
                    ErrorMessage = "Group not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var assignmentsQuery = _dbContext.GroupAssignments
                .Where(ga => ga.GroupId == requestModel.GroupId
                            && ga.TeacherId == requestModel.TeacherId
                            && !ga.IsDeleted)
                .AsNoTracking();

            var totalCount = await assignmentsQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)totalCount / requestModel.PageSize);

            var assignments = await assignmentsQuery
                .OrderBy(ga => ga.ExpiredAt)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .Select(ga => new StudentAssignmentPreviewModel
                {
                    AssignmentId = ga.Id,
                    TeacherName = ga.Teacher.Username,
                    TeacherId = ga.TeacherId,
                    TestName = ga.Test.Title,
                    ExpiredAt = ga.ExpiredAt
                })
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<StudentAssignmentPreviewModel>>(
                new PaginatedListResponseModel<StudentAssignmentPreviewModel>
                {
                    Items = assignments,
                    TotalPagesCount = pagesCount,
                    Page = requestModel.PageNumber,
                    PageSize = requestModel.PageSize,
                    HasPreviousPage = requestModel.PageNumber > 1,
                    HasNextPage = requestModel.PageNumber < pagesCount
                });
        }
        public async Task<OperationResult<PaginatedListResponseModel<TeacherAssignmentPreviewModel>>> GetIAssignedList(GetIAssignedRequestModel requestModel)
        {
            var studentAssignmentsQuery = _dbContext.StudentAssignments
                .Where(sa => sa.TeacherId == requestModel.UserId && sa.GroupAssignmentId == null && !sa.IsDeleted)
                .AsNoTracking()
                .Select(sa => new
                {
                    AssignmentId = sa.Id,
                    IsGroupAssignment = false,
                    RecipientName = sa.Student.Username,
                    RecipientId = sa.StudentId,
                    TestName = sa.Test.Title,
                    ExpiredAt = sa.ExpiredAt
                });

            var groupAssignmentsQuery = _dbContext.GroupAssignments
                .Where(ga => ga.TeacherId == requestModel.UserId && !ga.IsDeleted)
                .AsNoTracking()
                .Select(ga => new
                {
                    AssignmentId = ga.Id,
                    IsGroupAssignment = true,
                    RecipientName = ga.Group.Name,
                    RecipientId = ga.GroupId ?? 0,
                    TestName = ga.Test.Title,
                    ExpiredAt = ga.ExpiredAt
                });

            var totalStudentCount = await studentAssignmentsQuery.CountAsync();
            var totalGroupCount = await groupAssignmentsQuery.CountAsync();
            var totalCount = totalStudentCount + totalGroupCount;
            var pagesCount = (int)Math.Ceiling((double)totalCount / requestModel.PageSize);

            var allAssignments = await studentAssignmentsQuery
                .Concat(groupAssignmentsQuery)
                .OrderBy(a => a.ExpiredAt)
                .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .Select(a => new TeacherAssignmentPreviewModel
                {
                    AssignmentId = a.AssignmentId,
                    IsGroupAssignment = a.IsGroupAssignment,
                    RecipientName = a.RecipientName,
                    RecipientId = a.RecipientId,
                    TestName = a.TestName,
                    ExpiredAt = a.ExpiredAt
                })
                .ToListAsync();

            return new OperationResult<PaginatedListResponseModel<TeacherAssignmentPreviewModel>>(
                new PaginatedListResponseModel<TeacherAssignmentPreviewModel>
                {
                    Items = allAssignments,
                    TotalPagesCount = pagesCount,
                    Page = requestModel.PageNumber,
                    PageSize = requestModel.PageSize,
                    HasPreviousPage = requestModel.PageNumber > 1,
                    HasNextPage = requestModel.PageNumber < pagesCount
                });
        }
        public async Task<OperationResult> PatchAssignment(PatchAssignmentRequestModel requestModel)
        {
            var studentAssignment = await _dbContext.StudentAssignments
                .FirstOrDefaultAsync(sa => sa.Id == requestModel.AssignmentId && sa.TeacherId == requestModel.TeacherId && !sa.IsDeleted);
            if (studentAssignment != null)
            {
                if (requestModel.Deadline.HasValue) studentAssignment.ExpiredAt = requestModel.Deadline.Value;
                if (requestModel.Attempts.HasValue) studentAssignment.AttemptsLeft = (short)requestModel.Attempts.Value;
                studentAssignment.LastUpdateAt = DateTimeOffset.UtcNow;
                await _dbContext.SaveChangesAsync();
                return new OperationResult();
            }

            var groupAssignment = await _dbContext.GroupAssignments
                .Include(ga => ga.StudentAssignments.Where(sa => !sa.IsDeleted))
                .FirstOrDefaultAsync(ga => ga.Id == requestModel.AssignmentId && ga.TeacherId == requestModel.TeacherId && !ga.IsDeleted);
            if (groupAssignment != null)
            {
                if (requestModel.Deadline.HasValue)
                {
                    groupAssignment.ExpiredAt = requestModel.Deadline.Value;
                    foreach (var sa in groupAssignment.StudentAssignments)
                    {
                        sa.ExpiredAt = requestModel.Deadline.Value;
                        sa.LastUpdateAt = DateTimeOffset.UtcNow;
                    }
                }
                if (requestModel.Attempts.HasValue)
                {
                    groupAssignment.DefaultAttemptsAllowed = (short)requestModel.Attempts.Value;
                    foreach (var sa in groupAssignment.StudentAssignments)
                    {
                        sa.AttemptsLeft = (short)requestModel.Attempts.Value;
                        sa.LastUpdateAt = DateTimeOffset.UtcNow;
                    }
                }
                groupAssignment.LastUpdateAt = DateTimeOffset.UtcNow;
                await _dbContext.SaveChangesAsync();
                return new OperationResult();
            }

            return new OperationResult
            {
                ErrorCode = ErrorCodeConstants.AssignmentNotFound,
                ErrorMessage = "Assignment not found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        public async Task<OperationResult> RevokeAssignment(RevokeAssignmentRequestModel requestModel)
        {
            var studentAssignment = await _dbContext.StudentAssignments
                .FirstOrDefaultAsync(sa => sa.Id == requestModel.AssignmentId && sa.TeacherId == requestModel.TeacherId && !sa.IsDeleted);
            if (studentAssignment != null)
            {
                studentAssignment.IsDeleted = true;
                studentAssignment.LastUpdateAt = DateTimeOffset.UtcNow;
                await _dbContext.SaveChangesAsync();
                return new OperationResult();
            }

            var groupAssignment = await _dbContext.GroupAssignments
                .Include(ga => ga.StudentAssignments.Where(sa => !sa.IsDeleted))
                .FirstOrDefaultAsync(ga => ga.Id == requestModel.AssignmentId && ga.TeacherId == requestModel.TeacherId && !ga.IsDeleted);
            if (groupAssignment != null)
            {
                groupAssignment.IsDeleted = true;
                groupAssignment.LastUpdateAt = DateTimeOffset.UtcNow;

                foreach (var sa in groupAssignment.StudentAssignments)
                {
                    sa.IsDeleted = true;
                    sa.LastUpdateAt = DateTimeOffset.UtcNow;
                }

                await _dbContext.SaveChangesAsync();
                return new OperationResult();
            }

            return new OperationResult
            {
                ErrorCode = ErrorCodeConstants.AssignmentNotFound,
                ErrorMessage = "Assignment not found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        public async Task<OperationResult<StudentAssignmentDetailsModel>> GetStudentAssignmentDetails(GetAssignmentDetailsModel requestModel)
        {
            var assignment = await _dbContext.StudentAssignments
                .Where(sa => sa.Id == requestModel.AssignmentId && !sa.IsDeleted)
                .AsNoTracking()
                .Select(sa => new
                {
                    sa.StudentId,
                    sa.TeacherId,
                    Details = new StudentAssignmentDetailsModel
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
                    }
                })
                .FirstOrDefaultAsync();

            if (assignment == null)
            {
                return new OperationResult<StudentAssignmentDetailsModel>
                {
                    ErrorCode = ErrorCodeConstants.AssignmentNotFound,
                    ErrorMessage = "Assignment not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (requestModel.StudentId.HasValue && assignment.StudentId != requestModel.StudentId.Value)
            {
                return new OperationResult<StudentAssignmentDetailsModel>
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only view your own assignments",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            if (requestModel.TeacherId.HasValue && assignment.TeacherId != requestModel.TeacherId.Value)
            {
                return new OperationResult<StudentAssignmentDetailsModel>
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only view your own assignments",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            return new OperationResult<StudentAssignmentDetailsModel>(assignment.Details);
        }

        public async Task<OperationResult<GroupAssignmentDetailsModel>> GetGroupAssignmentDetails(GetAssignmentDetailsModel requestModel)
        {
            var assignment = await _dbContext.GroupAssignments
                .Where(ga => ga.Id == requestModel.AssignmentId && !ga.IsDeleted)
                .AsNoTracking()
                .Select(ga => new
                {
                    ga.TeacherId,
                    Details = new GroupAssignmentDetailsModel
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
                    }
                })
                .FirstOrDefaultAsync();

            if (assignment == null)
            {
                return new OperationResult<GroupAssignmentDetailsModel>
                {
                    ErrorCode = ErrorCodeConstants.AssignmentNotFound,
                    ErrorMessage = "Assignment not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (assignment.TeacherId != requestModel.TeacherId)
            {
                return new OperationResult<GroupAssignmentDetailsModel>
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only view your own assignments",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            return new OperationResult<GroupAssignmentDetailsModel>(assignment.Details);
        }

        public async Task<OperationResult<GroupScoreByAssignmentResponseModel>> GetGroupAssignmentScore(GetGroupAssignmentScoreModel requestModel)
        {
            var groupAssignment = await _dbContext.GroupAssignments
                .Include(ga => ga.StudentAssignments.Where(sa => !sa.IsDeleted))
                    .ThenInclude(sa => sa.Student)
                .FirstOrDefaultAsync(ga => ga.Id == requestModel.AssignmentId && ga.TeacherId == requestModel.TeacherId && !ga.IsDeleted);

            if (groupAssignment == null)
            {
                return new OperationResult<GroupScoreByAssignmentResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.AssignmentNotFound,
                    ErrorMessage = "Assignment not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var memberScores = new List<GroupMemberScoreByAssignmentResponseDto>();
            foreach (var sa in groupAssignment.StudentAssignments)
            {
                var bestAttempt = await _dbContext.TestAttempts
                    .Where(ta => ta.StudentId == sa.StudentId && ta.TestId == groupAssignment.TestId)
                    .OrderByDescending(ta => ta.RawScore)
                    .FirstOrDefaultAsync();

                memberScores.Add(new GroupMemberScoreByAssignmentResponseDto
                {
                    StudentId = sa.StudentId,
                    StudentName = sa.Student.Username,
                    IsPassed = bestAttempt?.RawScore >= 60,
                    PercentageScore = bestAttempt?.RawScore,
                    AttemptId = bestAttempt?.Id
                });
            }

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
                .FirstOrDefaultAsync(sa => sa.Id == requestModel.AssignmentId && !sa.IsDeleted);

            if (studentAssignment == null)
            {
                return new OperationResult<StudentScoreByAssignmentResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.AssignmentNotFound,
                    ErrorMessage = "Assignment not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            bool hasAccess = false;
            if (requestModel.TeacherId.HasValue)
            {
                hasAccess = studentAssignment.TeacherId == requestModel.TeacherId.Value;
            }
            if (requestModel.StudentId.HasValue)
            {
                hasAccess = studentAssignment.StudentId == requestModel.StudentId.Value;
            }

            if (!hasAccess)
            {
                return new OperationResult<StudentScoreByAssignmentResponseModel>
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only view your own assignment scores",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var attempts = await _dbContext.TestAttempts
                .Where(ta => ta.StudentId == studentAssignment.StudentId && ta.TestId == studentAssignment.TestId)
                .OrderByDescending(ta => ta.RawScore)
                .Select(ta => new StudentScoreByAssignmentItemResponseDto
                {
                    PercentageScore = ta.RawScore,
                    AttemptId = ta.Id,
                    Duration = (short)ta.Duration
                })
                .ToListAsync();

            var result = new StudentScoreByAssignmentResponseModel
            {
                Attempts = attempts,
                AttemptsLeft = studentAssignment.AttemptsLeft ?? 0,
                AveragePercentageScore = attempts.Any() ? (short?)attempts.Average(a => a.PercentageScore ?? 0) : null,
                BestPercentageScore = attempts.Any() ? attempts.Max(a => a.PercentageScore) : null
            };

            return new OperationResult<StudentScoreByAssignmentResponseModel>(result);
        }
    }
}