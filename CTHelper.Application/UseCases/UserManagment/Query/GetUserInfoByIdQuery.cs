using CTHelper.Application.Models;
using CTHelper.Application.Models.User;
using MediatR;

public record GetUserInfoByIdQuery(
    long UserId) : IRequest<OperationResult<GetUserInfoResponseModel>>;