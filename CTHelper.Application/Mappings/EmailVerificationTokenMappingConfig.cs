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
    }
}
