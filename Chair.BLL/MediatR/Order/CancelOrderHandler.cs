using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderQuery, Unit>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public CancelOrderHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<Unit> Handle(CancelOrderQuery request, CancellationToken cancellationToken)
        {
            await _orderBusinessLogic.CancelOrderAsync(request.OrderId);

            return Unit.Value;
        }
    }
}
