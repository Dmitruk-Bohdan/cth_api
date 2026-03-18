using CTHelper.Application.Models;
using CTHelper.Application.Models.Session;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Query;

public record GetMySessionListQuery(long UserId) : IRequest<OperationResult<List<UserSessionListResponseModel>>>;
