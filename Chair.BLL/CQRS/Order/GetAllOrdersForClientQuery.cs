using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class GetAllOrdersForClientQuery : IRequest<List<OrderDto>>
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
