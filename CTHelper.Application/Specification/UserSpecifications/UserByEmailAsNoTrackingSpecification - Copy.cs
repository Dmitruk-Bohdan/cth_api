using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications
{
    public class ActiveUserByEmailAsNoTrackingSpecification : BaseSpecification<User>
    {
        public ActiveUserByEmailAsNoTrackingSpecification(string email)
        {
            AddCriteria(u => u.Email == email);
            AddCriteria(u => u.IsDeleted == false);
            AsNoTracking = true;
        }
    }
}
