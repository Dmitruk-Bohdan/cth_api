using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Presentation.Dtos.AuthDtos;
using Mapster;

namespace CTHelper.Presentation.Mappings;

public class PasswordResetTokenDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ConfirmPasswordResetRequestDto, ConfirmPasswordResetCommand>()
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest.NewPassword, src => src.NewPassword);

        config.NewConfig<RequestPasswordResetRequestDto, RequestPasswordResetCommand>()
            .Map(dest => dest.UserEmail, src => src.Email);
    }
}
