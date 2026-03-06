using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Domain.Specification
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
