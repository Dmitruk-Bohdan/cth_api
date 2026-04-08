using CTHelper.Application.Models;
using CTHelper.Application.Models.Group;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Services.Interfaces
{
    public interface IGroupService
    {
        Task<OperationResult> AddStudentToGroup(AddStudentToGroupModel request);
        Task<OperationResult> CreateGroup(CreateGroupModel request);
        Task<OperationResult> DeleteGroup(DeleteGroupModel request);
        Task<OperationResult<GroupDetailsResponseModel>> GetGroupById(GetGroupByIdModel request);
        Task<OperationResult<PaginatedListResponseModel<GroupPreviewModel>>> GetMyGroupList(MyGroupListRequestModel request);
        Task<OperationResult> RemoveStudentFromGroup(RemoveStudentFromGroupModel request);
    }
}