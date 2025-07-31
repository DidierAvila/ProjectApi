using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DbContexts;
using Domain.Entities;

namespace DataAccess.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(JujuTestContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsByCustomerId(int CustomerId, CancellationToken cancellationToken)
        {
            return await Finds(post => post.CustomerId == CustomerId, cancellationToken)
                   ?? new List<Post>();
        }
    }
}
