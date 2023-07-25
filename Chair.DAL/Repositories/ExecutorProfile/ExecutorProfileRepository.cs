using Chair.DAL.Data;
using Chair.DAL.Repositories.Base;

namespace Chair.DAL.Repositories.ExecutorProfile
{
    public class ExecutorProfileRepository : BaseRepository<Data.Entities.ExecutorProfile>, IExecutorProfileRepository
    {
        public ExecutorProfileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
