using CTHelper.Application.Models.Dtos.AuthDtos;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Entities;
using Mapster;

namespace CTHelper.Application.Mappings;

public class EmailVerificationTokenMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, RequestEmailVerificationCommand>()
            .Map(dest => dest.UserEmail, src => src.Email);

        config.NewConfig<RequestEmailVerificationRequestDto, RequestEmailVerificationCommand>()
                    .Map(dest => dest.UserEmail, src => src.UserEmail);

        config.NewConfig<ConfirmEmailVerificationDto, ConfirmEmailVerificationCommand>()
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.TokenAsString, src => src.Token);
    }
}
