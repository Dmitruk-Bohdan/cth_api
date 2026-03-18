using CTHelper.Domain.Abstractions;

namespace CTHelper.Application.Specification.UserSession;

public class UserSessionByIdSpecification : BaseSpecification<Domain.Entities.UserSession>
{
    public UserSessionByIdSpecification(long id)
    {
        AddCriteria(s => s.Id == id);
    }
}
