using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Group;
using CTHelper.Application.Models.UserModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Group = CTHelper.Domain.Entities.Group;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileStorageService _fileStorageService;

        public GroupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OperationResult<PaginatedListResponseModel<GroupListItemModel>>> GetMyGroupList(MyGroupListRequestModel request)
        {
            var countQuery = _dbContext.Groups
                .Where(g =>
                    !g.IsDeleted
                    && g.TeacherId == request.TeacherId
                    && g.SubjectId == request.SubjectId)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.GroupName))
            {
                countQuery = countQuery.Where(g => g.Name.StartsWith(request.GroupName!));
            }

            var groupsCount = await countQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)groupsCount / request.PageSize);

            var groupPageList = await countQuery
                .Select(g => new GroupListItemModel()
                {
                    Name = g.Name,
                    CreatedAt = g.CreatedAt,
                    StudentsCount = g.Students.Count(),
                    GroupId = g.Id
                })
                .OrderBy(g => g.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var paginatedGroupList = new PaginatedListResponseModel<GroupListItemModel>()
            {
                Items = groupPageList,
                TotalPagesCount = pagesCount,
                Page = request.PageNumber,
                PageSize = request.PageSize,
                HasPreviousPage = request.PageNumber > 1,
                HasNextPage = request.PageNumber < pagesCount
            };

            var result = new OperationResult<PaginatedListResponseModel<GroupListItemModel>>(paginatedGroupList);
            return result;
        }

        public async Task<OperationResult> CreateGroup(CreateGroupModel request)
        {
            var newGroup = new Group()
            {
                SubjectId = request.SubjectId,
                TeacherId = request.TeacherId,
                Name = request.GroupName
            };

            await _dbContext.Groups.AddAsync(newGroup);
            _dbContext.SaveChanges();

            return new OperationResult();
        }

        public async Task<OperationResult> DeleteGroup(DeleteGroupModel request)
        {
            var groupToDelete = await _dbContext.Groups
                .Where(g => g.Id == request.GroupId)
                .FirstOrDefaultAsync();

            if (groupToDelete == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.GroupNotFound,
                    ErrorMessage = "Specified group is not found",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            if (groupToDelete.TeacherId != request.TeacherId)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            groupToDelete.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> AddStudentToGroup(AddStudentToGroupModel request)
        {
            var studentBelongToTeacher = await _dbContext.TeacherStudents
                .Where(ts =>
                    ts.TeacherId == request.TeacherId
                    && ts.StudentId == request.StudentId)
                .AnyAsync();

            if (!studentBelongToTeacher)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var groupBelongToTeacher = await _dbContext.Groups
                .Where(g =>
                    g.Id == request.GroupId
                    && g.TeacherId == request.TeacherId)
                .AnyAsync();

            if (!groupBelongToTeacher)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var group = await _dbContext.Groups
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == request.GroupId);

            if (group == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.GroupNotFound,
                    ErrorMessage = "Group not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var student = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == request.StudentId && !u.IsDeleted);

            if (student == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.UserNotFound,
                    ErrorMessage = "Student not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (group.Students.Any(s => s.Id == request.StudentId))
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.StudentAlreadyInGroup,
                    ErrorMessage = "Student is already in this group",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            group.Students.Add(student);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> RemoveStudentFromGroup(RemoveStudentFromGroupModel request)
        {
            var studentBelongToTeacher = await _dbContext.TeacherStudents
                .Where(ts =>
                    ts.TeacherId == request.TeacherId
                    && ts.StudentId == request.StudentId)
                .AnyAsync();

            if (!studentBelongToTeacher)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var groupBelongToTeacher = await _dbContext.Groups
                .Where(g =>
                    g.Id == request.GroupId
                    && g.TeacherId == request.TeacherId)
                .AnyAsync();

            if (!groupBelongToTeacher)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var group = await _dbContext.Groups
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == request.GroupId);

            if (group == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.GroupNotFound,
                    ErrorMessage = "Group not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var student = group.Students.FirstOrDefault(s => s.Id == request.StudentId);

            if (student == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.StudentNotBelongToGroup,
                    ErrorMessage = "Student does not belong to this group",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            group.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }
        public async Task<OperationResult<GroupDetailsResponseModel>> GetGroupById(GetGroupByIdModel request)
        {
            var group = await _dbContext.Groups
                .Where(g => g.Id == request.GroupId && !g.IsDeleted)
                .Select(g => new
                {
                    g.Id,
                    g.Name,
                    g.SubjectId,
                    SubjectName = g.Subject.Name,
                    g.TeacherId,
                    TeacherName = g.Teacher.Username,
                    Students = g.Students
                        .Where(s => !s.IsDeleted)
                        .Select(s => new UserProfilePreviewWithAvatarIdModel
                        {
                            UserId = s.Id,
                            Username = s.Username,
                            AvatarId = s.AvatarImageId
                        })
                        .ToList(),
                    CreatedAt = g.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (group == null)
            {
                return new OperationResult<GroupDetailsResponseModel>()
                {
                    ErrorCode = ErrorCodeConstants.GroupNotFound,
                    ErrorMessage = "Group not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (group.TeacherId != request.TeacherId)
            {
                return new OperationResult<GroupDetailsResponseModel>()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only view your own groups",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var studentsWithAvatars = await Task.WhenAll(group.Students.Select(async student => new UserProfilePreviewModel
            {
                UserId = student.UserId,
                Username = student.Username,
                AvatarUrl = student.AvatarId == null ? null : await _fileStorageService.GetDownloadUrl(student.AvatarId.Value)
            }));

            var response = new GroupDetailsResponseModel
            {
                GroupId = group.Id,
                Name = group.Name,
                SubjectId = group.SubjectId,
                SubjectName = group.SubjectName,
                TeacherId = group.TeacherId,
                TeacherName = group.TeacherName,
                Students = studentsWithAvatars.ToList(),
                CreatedAt = group.CreatedAt
            };

            return new OperationResult<GroupDetailsResponseModel>(response);
        }
    }
}
