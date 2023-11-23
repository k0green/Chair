using Chair.BLL.Dto.Chat;
using MediatR;

namespace Chair.BLL.CQRS.Chat
{
    public class GetChatForProfileQuery : IRequest<ChatDto>
    {
        public Guid ProfileId { get; set; }
    }
}
