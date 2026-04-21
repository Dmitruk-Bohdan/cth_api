using CTHelper.Application.Models.UserModels;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications;
public class UserIsMailVerifiedModelByUserEmailSpecification : BaseSpecification<User, UserMailVerifiedModel>
{
    public UserIsMailVerifiedModelByUserEmailSpecification(string email)
    {
        ApplySelector(u => new UserMailVerifiedModel
        {
            UserId = u.Id,
            IsEmailVerified = u.IsEmailVerified,
        });

        AddCriteria(u => u.Email == email);
    }
}
