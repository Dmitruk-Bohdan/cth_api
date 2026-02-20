using CTHelper.Application.Models.Dtos.AuthDtos;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Domain.Entities;
using Mapster;

namespace CTHelper.Application.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterUserRequestDto, CreateUserCommand>()
            .Map(dest => dest.Username, src => src.Username)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.Role, src => src.Role.ToEnum<UserRole>());

        config.NewConfig<CreateUserCommand, User>()
            .Map(dest => dest.Username, src => src.Username)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Role, src => src.Role);
    }
}