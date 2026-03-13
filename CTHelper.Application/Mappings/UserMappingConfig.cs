using CTHelper.Application.Models.Dtos.AuthDtos;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Domain.Entities;
using Mapster;

namespace CTHelper.Application.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterUserRequestDto, RegisterUserCommand>()
            .Map(dest => dest.Username, src => src.Username)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.Role, src => src.Role);

        config.NewConfig<RegisterUserCommand, User>()
            .Map(dest => dest.Username, src => src.Username)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.IsEmailVerified, src => false)
            .Map(dest => dest.Role, src => src.Role);

        config.NewConfig<LoginRequestDto, LoginCommand>()
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.ClientType, src => src.ClientType)
            .Map(dest => dest.IpAddress, src => src.IpAddress)
            .Map(dest => dest.DeviceInfo, src => src.DeviceInfo)
            .Map(dest => dest.DeviceId, src => src.DeviceId);

        config.NewConfig<RefreshTokenRequestDto, RefreshTokenCommand>()
            .Map(dest => dest.RefreshToken, src => src.RefreshToken);

        config.NewConfig<LogoutRequestDto, LogoutCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.SessionJti, src => src.SessionJti);
    }
}
