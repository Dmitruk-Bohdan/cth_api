using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Models.User;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Domain.Abstractions;
using MapsterMapper;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class UserManagmentService : IUserManagmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public UserManagmentService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
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

        public async Task<OperationResult> UpdateUserProfileAsync(UpdateUserProfileModel updateUser)
        {
            var user = await _unitOfWork.Users.GetAsync(new UserByIdSpecification(updateUser.UserId));
            if(user == null)
            {
                return OperationResultHelper.UserNotFoundTemplate(id: updateUser.UserId);
            }
            
            user.Username = updateUser.Username!;

            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult<UserProfileResponseModel>> GetUserInfoById(long userId)
        {
            var user = await _unitOfWork.Users.GetAsync(new ActiveUserAsNoTrackingByIdSpecification(userId));
            if (user == null)
            {
                return OperationResultHelper.UserNotFoundTemplate<UserProfileResponseModel>(id: userId);
            }

            var userInfoResponse = _mapper.Map<UserProfileResponseModel>(user);

            if (user.AvatarImageId != null)
            {
                var avatarLink = await _fileStorageService.GetDownloadUrl(user.AvatarImageId!.Value);
                userInfoResponse.AvatarUrl = avatarLink;
            }

            return new OperationResult<UserProfileResponseModel>(userInfoResponse);
        }
    }
}
