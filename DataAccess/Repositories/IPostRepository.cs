using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Domain.Entities;

namespace DataAccess.Repositories
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        Task<IEnumerable<Post>> GetPostsByCustomerId(int CustomerId, CancellationToken cancellationToken);
    }
}