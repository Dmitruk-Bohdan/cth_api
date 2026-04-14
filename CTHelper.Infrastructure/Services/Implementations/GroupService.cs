using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Group;
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

        public GroupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OperationResult<PaginatedListResponseModel<GroupPreviewModel>>> GetMyGroupList(MyGroupListRequestModel request)
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
                .Select(g => new GroupPreviewModel()
                {
                    Name = g.Name,
                    CreatedAt = g.CreatedAt,
                    StudentsCount = g.Students.Count(),
                })
                .OrderBy(g => g.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var paginatedGroupList = new PaginatedListResponseModel<GroupPreviewModel>()
            {
                Items = groupPageList,
                TotalPagesCount = pagesCount,
                Page = request.PageNumber,
                PageSize = request.PageSize,
                HasPreviousPage = request.PageNumber > 1,
                HasNextPage = request.PageNumber < pagesCount
            };

            var result = new OperationResult<PaginatedListResponseModel<GroupPreviewModel>>(paginatedGroupList);
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

            var groupStub = new Group { Id = request.GroupId };
            var studentStub = new User { Id = request.StudentId };

            _dbContext.Groups.Attach(groupStub);
            _dbContext.Users.Attach(studentStub);

            groupStub.Students.Add(studentStub);
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

            var groupStub = new Group { Id = request.GroupId };
            var studentStub = new User { Id = request.StudentId };

            _dbContext.Groups.Attach(groupStub);
            _dbContext.Users.Attach(studentStub);

            var studentBelongToGroup = await _dbContext.Groups
                .Where(g =>
                    g.Id == request.GroupId
                    && g.Students.Contains(studentStub)
                    && g.TeacherId == request.TeacherId)
                .AnyAsync();

            if (!studentBelongToGroup)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.StudentNotBelongToGroup,
                    ErrorMessage = "Specified student do not belond to specified group",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            groupStub.Students.Remove(studentStub);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult<GroupDetailsResponseModel>> GetGroupById(GetGroupByIdModel request)
        {
            return new OperationResult<GroupDetailsResponseModel>();
        }
    }
}
