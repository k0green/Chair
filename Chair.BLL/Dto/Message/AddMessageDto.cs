using Chair.DAL.Enums;

namespace Chair.BLL.Dto.Message;

public class AddMessageDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsEdited { get; set; }
    public bool IsRead { get; set; }
    public MessageType Type { get; set; }

    public Guid ChatId { get; set; }
    public Guid? ReplyId { get; set; }
    public string RecipientId { get; set; }
    public string SenderId { get; set; }
}