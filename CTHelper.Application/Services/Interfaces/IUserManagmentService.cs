using CTHelper.Application.Models;
using CTHelper.Application.Models.UserModels;

namespace CTHelper.Application.Services.Interfaces
{
    public interface IUserManagmentService
    {
        Task<OperationResult> DeleteUserAsync(long UserId);
        Task<OperationResult> DeleteUserAvatarAsync(long UserId);
        Task<OperationResult> UpdateUserAvatarAsync(long userId, long imageId);
        Task<OperationResult> UpdateUserProfileAsync(UpdateUserProfileModel updatedUser);
        Task<OperationResult<UserProfileResponseModel>> GetUserInfoById(long userId);
    }
}