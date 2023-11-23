using MediatR;

namespace Chair.BLL.CQRS.Chat
{
    public class RemoveChatQuery : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
