using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class GetAllOrdersByServiceIdQuery : IRequest<List<OrderDto>>
    {
        public Guid ExecutorServiceId { get; set; }
    }
}
