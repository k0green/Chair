using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.Message;

namespace Chair.BLL.BusinessLogic.Message
{
    public interface IMessageBusinessLogic
    {
        Task<Guid> AddAsync(AddMessageDto dto);
        Task<Guid> EditText(LookupDto dto);
        Task MarkAsRead(Guid chatId);
        Task RemoveAsync(Guid id);
    }
}
