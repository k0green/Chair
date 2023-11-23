using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.Message;
using Chair.DAL.Data.Entities;

namespace Chair.BLL.Dto.Chat;

public class ChatDto : BaseDto
{
    public string RecipientId { get; set; }
    public string SenderId { get; set; }
    public string RecipientName { get; set; }
    public Guid RecipientProfileId { get; set; }
    public string RecipientProfileImg { get; set; }
    public int UnreadMessagesAmount { get; set; }
    public List<MessageDto> Messages { get; set; }
    public bool IsDeleted { get; set; }
}