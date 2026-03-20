using CTHelper.Application.UseCases.TeacherStudentRelationship.Command;
using CTHelper.Presentation.Dtos.TeacherStudentDtos;
using Mapster;

namespace CTHelper.Presentation.Mappings;

public class TeacherStudentDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateInvitationCodeRequestDto, CreateInvitationCodeCommand>()
            .Map(dest => dest.ExpiredAt, src => src.ExpiredAt == null ? (DateTimeOffset?)null : DateTimeOffset.Parse(src.ExpiredAt))
            .Map(dest => dest.UsesCount, src => src.UsesCount);
    }
}