using CTHelper.Application.Models.User;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications;

public class UserTokenByEmailAsNoTrackingSpecification : BaseSpecification<User, UserTokenModel>
{
    public UserTokenByEmailAsNoTrackingSpecification(string email)
    {
        AddCriteria(u => u.Email == email);
        ApplySelector(u => new UserTokenModel
        {
            UserId = u.Id,
            PasswordHash = u.PasswordHash,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role
        });
        AsNoTracking = true;
    }
}
