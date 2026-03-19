using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command;

public class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, OperationResult>
{
    private readonly IUserManagmentService _userManagmentService;
    private readonly IFileStorageService _fileStorage;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAvatarCommandHandler(IUserManagmentService userManagmentService, IUnitOfWork unitOfWork, IFileStorageService fileStorage)
    {
        _userManagmentService = userManagmentService;
        _unitOfWork = unitOfWork;
        _fileStorage = fileStorage;
    }

    public async Task<OperationResult> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        var keyPrefix = $"{FileStorageConstants.UsersSubfolderName}/{request.UserId}";
        var avatarObjectKey = await _fileStorage.UploadAsync(
            request.UserAvatarStream,
            keyPrefix,
            FileStorageConstants.AvatarBucketName,
            request.ContentType);

        var newImageDbEntity = new Image()
        {
            OwnerId = request.UserId,
            Bucket = FileStorageConstants.AvatarBucketName,
            ObjectKey = avatarObjectKey,
            Size = request.UserAvatarStream.Length,
            ContentType = request.ContentType
        };

        await _unitOfWork.Images.AddAsync(newImageDbEntity);
        await _unitOfWork.SaveChangesAsync();

        return await _userManagmentService.UpdateUserAvatarAsync(newImageDbEntity.OwnerId, newImageDbEntity.Id);
    }
}