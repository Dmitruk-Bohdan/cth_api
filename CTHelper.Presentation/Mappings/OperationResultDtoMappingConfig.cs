using CTHelper.Application.Models;
using CTHelper.Presentation.Dtos;
using Mapster;

namespace CTHelper.Presentation.Mappings
{
    public class OperationResultDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<OperationResult, ErrorResponseDto>()
                .Map(dest => dest.ErrorCode, src => src.ErrorCode)
                .Map(dest => dest.ErrorMessage, src => src.ErrorMessage);
        }
    }
}
