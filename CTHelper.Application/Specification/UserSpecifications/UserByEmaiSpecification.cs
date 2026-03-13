using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications
{
    public class UserByEmaiSpecification : BaseSpecification<User>
    {
        public UserByEmaiSpecification(string email)
        {
            AddCriteria(u => u.Email == email);
        }
    }
}
