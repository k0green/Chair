using Chair.DAL.Data;
using Chair.DAL.Repositories.Base;

namespace Chair.DAL.Repositories.ExecutorService
{
    public class ServiceTypeRepository : BaseRepository<Data.Entities.ServiceType>, IServiceTypeRepository
    {
        public ServiceTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
