using Chair.BLL.Dto.Chat;
using MediatR;

namespace Chair.BLL.CQRS.Chat
{
    public class GetAllChatForCurrentUserQuery : IRequest<List<ChatDto>>
    {
    }
}
