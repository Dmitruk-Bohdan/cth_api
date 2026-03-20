using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications
{
    public class UserByIdAsNoTrackingSpecification : BaseSpecification<User>
    {
        public UserByIdAsNoTrackingSpecification(long userId)
        {
            AddCriteria(u => u.Id == userId);
            AsNoTracking = true;
        }
    }
}
