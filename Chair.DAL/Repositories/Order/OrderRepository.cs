using Chair.DAL.Data;
using Chair.DAL.Repositories.Base;

namespace Chair.DAL.Repositories.Order
{
    public class OrderRepository : BaseWithManyRepository<Data.Entities.Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
