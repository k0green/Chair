using Chair.DAL.Data;
using Chair.DAL.Repositories.Base;
using Chair.DAL.Repositories.Message;

namespace Chair.DAL.Repositories.Order
{
    public class MessageRepository : BaseWithManyRepository<Data.Entities.Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
