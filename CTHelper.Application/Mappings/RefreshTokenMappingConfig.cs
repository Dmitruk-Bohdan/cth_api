using CTHelper.Application.Models.Dtos.AuthDtos;
using CTHelper.Application.UseCases.Identity.Command;
using Mapster;

namespace CTHelper.Application.Mappings;

public class RefreshTokenMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<RefreshTokenRequestDto, RefreshTokenCommand>()
        //    .Map(dest => dest.RefreshToken, src => src.RefreshToken)
        //    .Map(dest => dest.SessionJti, src =>
        //    string.IsNullOrWhiteSpace(src.SessionsJwt) ?
        //    Guid.Empty :
        //    Guid.Parse(src.SessionsJwt!));
    }
}
