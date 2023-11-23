using MediatR;

namespace Chair.BLL.CQRS.Message
{
    public class RemoveMessageQuery : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
