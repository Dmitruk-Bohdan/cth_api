using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Domain.Specification
{
    public class UserByIdSpecification : BaseSpecification<User>
    {
        public UserByIdSpecification(long id)
        {
            AddCriteria(u => u.Id == id);
        }
    }
}
