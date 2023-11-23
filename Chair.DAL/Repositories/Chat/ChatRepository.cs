using Chair.DAL.Data;
using Chair.DAL.Repositories.Base;
using Chair.DAL.Repositories.Chat;

namespace Chair.DAL.Repositories.Order
{
    public class ChatRepository : BaseWithManyRepository<Data.Entities.Chat>, IChatRepository
    {
        public ChatRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
