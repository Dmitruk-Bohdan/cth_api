using CTHelper.Application.Models;
using CTHelper.Application.UseCases.Identity.Command.ResponseModels;
using CTHelper.Domain.Common.Enums;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record LoginCommand(
    string Email,
    string Password,
    ClientTypeEnum ClientType,
    string? IpAddress,
    string? DeviceInfo,
    string? DeviceId) : IRequest<OperationResult<LoginResponseModel>>;


