using CTHelper.Application.Models.Dtos.AuthDtos;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Entities;
using Mapster;

namespace CTHelper.Application.Mappings;

public class UserSessionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserSession, UserSessionDto>()
            .Map(dest => dest.Jti, src => src.Jti)
            .Map(dest => dest.ClientType, src => src.ClientType)
            .Map(dest => dest.IpAddress, src => src.IpAddress)
            .Map(dest => dest.DeviceInfo, src => src.DeviceInfo)
            .Map(dest => dest.LastActivityAt, src => src.LastActivityAt)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);
    }
}