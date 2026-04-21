using CTHelper.Application.Models.UserModels;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications;

public class UserPasswordByEmailAsNoTrackingSpecification : BaseSpecification<User, UserPasswordModel>
{
    public UserPasswordByEmailAsNoTrackingSpecification(string email)
    {
        AddCriteria(u => u.Email == email);
        ApplySelector(u => new UserPasswordModel
        {
            Id = u.Id,
            PasswordHash = u.PasswordHash
        });
        AsNoTracking = true;
    }
}
