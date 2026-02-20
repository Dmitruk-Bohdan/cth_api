using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Domain.Specification
{
    public class UserByEmailSpecification : BaseSpecification<User>
    {
        public UserByEmailSpecification(string email)
        {
            AddCriteria(u => u.Email == email);
        }
    }
}
