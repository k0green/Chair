using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class GetAllOrdersForExecutorQuery : IRequest<List<OrderDto>>
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
