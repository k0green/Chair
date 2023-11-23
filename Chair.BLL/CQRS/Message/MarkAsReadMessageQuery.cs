using MediatR;
using Chair.BLL.Dto.ExecutorService;

namespace Chair.BLL.CQRS.ExecutorService
{
    public class MarkAsReadMessageQuery : IRequest<Unit>
    {
        public Guid ChatId { get; set; }
    }
}
