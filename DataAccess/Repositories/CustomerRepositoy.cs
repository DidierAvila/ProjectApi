using DataAccess.DbContexts;
using Domain.Entities;

namespace DataAccess.Repositories
{
    public class CustomerRepositoy : RepositoryBase<Customer>, ICustomerRepositoy
    {
        public CustomerRepositoy(JujuTestContext context) : base(context)
        {
        }
    }
}
