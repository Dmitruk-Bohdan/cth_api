using CTHelper.Application.Models.User;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications;
public class UserMailModelAsNoTrackingByUserEmailSpecification : BaseSpecification<User, UserMailModel>
{
    public UserMailModelAsNoTrackingByUserEmailSpecification(string email)
    {
        ApplySelector(u => new UserMailModel
        {
            UserId = u.Id,
            Email = u.Email,
        });

        AddCriteria(u => u.Email == email);
    }
}
