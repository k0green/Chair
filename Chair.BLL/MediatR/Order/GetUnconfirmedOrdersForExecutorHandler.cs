using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class GetUnconfirmedOrdersForExecutorHandler : IRequestHandler<GetUnconfirmedOrdersForExecutorQuery, List<OrderDto>>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public GetUnconfirmedOrdersForExecutorHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<List<OrderDto>> Handle(GetUnconfirmedOrdersForExecutorQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderBusinessLogic.GetUnconfirmedOrdersForExecutor();

            return result;
        }
    }
}
