using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class UpdateOrderQuery : IRequest<Unit>
    {
        public UpdateOrderDto UpdateOrderDto { get; set; }
    }
}
