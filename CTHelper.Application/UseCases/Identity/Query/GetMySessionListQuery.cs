using CTHelper.Application.Models.Dtos.AuthDtos;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Query;

public class GetMySessionListQuery : IRequest<List<UserSessionDto>>
{
    public long UserId { get; set; }
}
