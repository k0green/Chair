using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class GetOrderByIdQuery : IRequest<OrderDto>
    {
        public Guid Id { get; set; }
    }
}
