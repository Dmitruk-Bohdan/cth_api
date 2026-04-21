using CTHelper.Application.Models.UserModels;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications;

public class UserLoginByEmailAsNoTrackingSpecification : BaseSpecification<User, UserLoginModel>
{
    public UserLoginByEmailAsNoTrackingSpecification(string email)
    {
        AddCriteria(u => u.Email == email);
        ApplySelector(u => new UserLoginModel
        {
            UserId = u.Id,
            PasswordHash = u.PasswordHash,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role,
            IsEmailVerified = u.IsEmailVerified
        });
        AsNoTracking = true;
    }
}
