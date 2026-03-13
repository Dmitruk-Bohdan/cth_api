using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications
{
    public class UserByEmailAsNoTrackingSpecification : BaseSpecification<User>
    {
        public UserByEmailAsNoTrackingSpecification(string email)
        {
            AddCriteria(u => u.Email == email);
            AsNoTracking = true;
        }
    }
}
