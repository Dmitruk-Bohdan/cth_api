using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications
{
    public class UserByIdAsNoTrackingSpecification : BaseSpecification<User>
    {
        public UserByIdAsNoTrackingSpecification(string email)
        {
            AddCriteria(u => u.Email == email);
            AsNoTracking = true;
        }
    }
}
