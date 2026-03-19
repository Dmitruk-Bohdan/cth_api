using CTHelper.Application.Models.User;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Application.UseCases.Identity.Command.ResponseModels;
using CTHelper.Application.UseCases.UserManagment.Command;
using CTHelper.Presentation.Dtos.AuthDtos;
using CTHelper.Presentation.Dtos.UserDtos;
using Mapster;

namespace CTHelper.Presentation.Mappings;

public class UserDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterUserRequestDto, RegisterUserCommand>()
            .Map(dest => dest.Username, src => src.Username)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.Role, src => src.Role);

        config.NewConfig<LoginRequestDto, LoginCommand>()
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.ClientType, src => src.ClientType)
            .Map(dest => dest.IpAddress, src => src.IpAddress)
            .Map(dest => dest.DeviceInfo, src => src.DeviceInfo)
            .Map(dest => dest.DeviceId, src => src.DeviceId);

        config.NewConfig<LoginResponseModel, LoginResponseDto>()
            .Map(dest => dest.AccessToken, src => src.AccessToken);

        config.NewConfig<LogoutRequestDto, LogoutCommand>()
            .Map(dest => dest.SessionJti, src => src.SessionJti);

        config.NewConfig<UpdateUserProfileRequestDto, UpdateUserProfileCommand>()
                    .Map(dest => dest.Username, src => src.Username);

        config.NewConfig<UpdateUserProfileCommand, UpdateUserProfileModel>()
                            .Map(dest => dest.Username, src => src.Username);
    }
}
