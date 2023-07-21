using Chair.DAL.Data;
using Chair.DAL.Repositories.Base;

namespace Chair.DAL.Repositories.ExecutorService
{
    public class ExecutorServiceRepository : BaseRepository<Data.Entities.ExecutorService>, IExecutorServiceRepository
    {
        public ExecutorServiceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
