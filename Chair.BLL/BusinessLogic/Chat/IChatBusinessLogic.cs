using Chair.BLL.Dto.Chat;

namespace Chair.BLL.BusinessLogic.Chat
{
    public interface IChatBusinessLogic
    {
        Task<List<ChatDto>> GetAllChatForCurrentUser();
        Task<ChatDto> GetChatForProfile(Guid profileId);
        Task RemoveAsync(Guid id);
    }
}
