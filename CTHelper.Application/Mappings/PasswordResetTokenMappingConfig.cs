using CTHelper.Application.Models.Dtos.AuthDtos;
using CTHelper.Application.UseCases.Identity.Command;
using Mapster;

namespace CTHelper.Application.Mappings;

public class PasswordResetTokenMappingConfig : IRegister
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
