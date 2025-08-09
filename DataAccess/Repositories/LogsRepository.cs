using DataAccess.DbContexts;
using Domain.Entities;

namespace DataAccess.Repositories
{
    public class LogsRepository : RepositoryBase<Logs>, ILogsRepository
    {
        public LogsRepository(JujuTestContext context) : base(context)
        {
        }
    }
}