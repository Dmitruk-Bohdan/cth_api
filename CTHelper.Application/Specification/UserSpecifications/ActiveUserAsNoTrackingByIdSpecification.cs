using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications
{
    public class ActiveUserAsNoTrackingByIdSpecification : BaseSpecification<User>
    {
        public ActiveUserAsNoTrackingByIdSpecification(long id)
        {
            AddCriteria(u => u.Id == id);
            AddCriteria(u => u.IsDeleted == false);
            AsNoTracking = true;
        }
    }
}
