using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class AddOrderQuery : IRequest<List<Guid>>
    {
        public List<AddOrderDto> AddOrderDtos { get; set; }
    }
}
