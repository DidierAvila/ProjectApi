using DataAccess.DbContexts;
using Domain.Entities;

namespace DataAccess.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(JujuTestContext context) : base(context)
        {
        }
    }
}
