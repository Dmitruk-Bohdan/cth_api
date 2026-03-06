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
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest.UserEmail, src => src.Email);

        config.NewConfig<ConfirmEmailVerificationDto, ConfirmEmailVerificationCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.TokenAsString, src => src.Token);
    }
}