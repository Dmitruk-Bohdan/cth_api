using CTHelper.Application.Models;
using CTHelper.Application.Models.UserModels;
using MediatR;

public record GetUserInfoByIdQuery(
    long UserId) : IRequest<OperationResult<UserProfileResponseModel>>;