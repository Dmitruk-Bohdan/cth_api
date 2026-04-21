using CTHelper.Application.Models.UserModels;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications;

public class UserIdByEmailAsNoTrackingSpecification : BaseSpecification<User, UserIdModel>
{
    public UserIdByEmailAsNoTrackingSpecification(string email)
    {
        AddCriteria(u => u.Email == email);
        ApplySelector(u => new UserIdModel { UserId = u.Id });
        AsNoTracking = true;
    }
}


