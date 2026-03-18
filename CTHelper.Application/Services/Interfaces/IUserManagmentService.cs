using CTHelper.Application.Models;
using CTHelper.Application.Models.User;

namespace CTHelper.Application.Services.Interfaces
{
    public interface IUserManagmentService
    {
        Task<OperationResult> DeleteUserAsync(long UserId);
        Task<OperationResult> DeleteUserAvatarAsync(long UserId);
        Task<OperationResult> UpdateUserAvatarAsync(long userId, long avatarImageId);
        Task<OperationResult> UpdateUserInfoAsync(UpdateUserModel updatedUser);
    }
}