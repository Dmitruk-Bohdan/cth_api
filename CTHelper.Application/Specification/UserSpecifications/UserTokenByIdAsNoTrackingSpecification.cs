using CTHelper.Application.Models.User;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications;

public class UserTokenByIdAsNoTrackingSpecification : BaseSpecification<User, UserTokenModel>
{
    public UserTokenByIdAsNoTrackingSpecification(long userId)
    {
        AddCriteria(u => u.Id == userId);
        ApplySelector(u => new UserTokenModel
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role
        });
        AsNoTracking = true;
    }
}
