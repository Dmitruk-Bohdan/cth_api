using CTHelper.Application.Models;
using CTHelper.Application.Models.Dtos;
using Mapster;

namespace CTHelper.Application.Mappings
{
    public class OperationResultMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<OperationResult, ErrorResponseDto>()
                .Map(dest => dest.ErrorCode, src => src.ErrorCode)
                .Map(dest => dest.ErrorMessage, src => src.ErrorMessage);
        }
    }
}
