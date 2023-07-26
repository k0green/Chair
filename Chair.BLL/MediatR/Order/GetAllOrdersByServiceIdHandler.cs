using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class GetAllOrdersByServiceIdHandler : IRequestHandler<GetAllOrdersByServiceIdQuery, List<OrderDto>>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public GetAllOrdersByServiceIdHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersByServiceIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderBusinessLogic.GetAllOrdersByServiceId(request.ExecutorServiceId);

            return result;
        }
    }
}
