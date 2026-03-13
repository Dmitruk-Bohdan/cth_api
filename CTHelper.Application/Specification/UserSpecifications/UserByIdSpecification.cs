using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.UserSpecifications
{
    public class UserByIdSpecification : BaseSpecification<User>
    {
        public UserByIdSpecification(long id)
        {
            AddCriteria(u => u.Id == id);
        }
    }
}
