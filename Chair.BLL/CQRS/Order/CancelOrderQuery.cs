using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class CancelOrderQuery : IRequest<Unit>
    {
        public Guid OrderId { get; set; }
    }
}
