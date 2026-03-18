using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Presentation.Dtos.AuthDtos;
using Mapster;

namespace CTHelper.Presentation.Mappings;

public class EmailVerificationTokenDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RequestEmailVerificationRequestDto, RequestEmailVerificationCommand>()
                    .Map(dest => dest.UserEmail, src => src.UserEmail);

        config.NewConfig<ConfirmEmailVerificationDto, ConfirmEmailVerificationCommand>()
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.TokenAsString, src => src.Token);
    }
}
