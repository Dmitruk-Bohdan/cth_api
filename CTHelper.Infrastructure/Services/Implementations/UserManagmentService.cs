using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Models.User;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Domain.Abstractions;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class UserManagmentService : IUserManagmentService
    {
        private IUnitOfWork _unitOfWork;

        public UserManagmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> DeleteUserAsync(long userId)
        {
            var user = await _unitOfWork.Users.GetAsync(new UserByIdSpecification(userId));
            if (user == null)
            {
                return OperationResultHelper.UserNotFoundTemplate(id: userId);
            }

            user!.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> UpdateUserAvatarAsync(long userId, long avatarImageId)
        {
            var user = await _unitOfWork.Users.GetAsync(new UserByIdSpecification(userId));
            if (user == null)
            {
                return OperationResultHelper.UserNotFoundTemplate(id: userId);
            }

            user!.AvatarImageId = avatarImageId;
            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> DeleteUserAvatarAsync(long userId)
        {
            var user = await _unitOfWork.Users.GetAsync(new UserByIdSpecification(userId));
            if(user == null)
            {
                return OperationResultHelper.UserNotFoundTemplate(id: userId);
            }

            user!.AvatarImageId = null;
            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> UpdateUserInfoAsync(UpdateUserModel updateUser)
        {
            var user = await _unitOfWork.Users.GetAsync(new UserByIdSpecification(updateUser.UserId));
            if(user == null)
            {
                return OperationResultHelper.UserNotFoundTemplate(id: updateUser.UserId);
            }
            
            if(string.IsNullOrWhiteSpace(updateUser.Username))
            {
                user.Username = updateUser.Username!;
            }
            if(updateUser.UserAvatarId != null)
            {
                user.AvatarImageId = updateUser.UserAvatarId;
            }

            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }
    }
}
