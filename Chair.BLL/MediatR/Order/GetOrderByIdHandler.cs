using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public GetOrderByIdHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderBusinessLogic.GetOrderById(request.Id);

            return result;
        }
    }
}
