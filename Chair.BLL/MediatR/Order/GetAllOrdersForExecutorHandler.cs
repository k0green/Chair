using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class GetAllOrdersForExecutorHandler : IRequestHandler<GetAllOrdersForExecutorQuery, List<OrderDto>>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public GetAllOrdersForExecutorHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersForExecutorQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderBusinessLogic.GetAllOrdersForExecutor(request.Month, request.Year);

            return result;
        }
    }
}
