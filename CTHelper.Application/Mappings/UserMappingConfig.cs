using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Entities;
using Mapster;

namespace CTHelper.Application.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterUserCommand, User>()
            .Map(dest => dest.Username, src => src.Username)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.IsEmailVerified, src => false)
            .Map(dest => dest.Role, src => src.Role);

        config.NewConfig<User, RequestEmailVerificationCommand>()
            .Map(dest => dest.UserEmail, src => src.Email);
    }
}
