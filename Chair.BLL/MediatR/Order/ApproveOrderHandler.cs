using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class ApproveOrderHandler : IRequestHandler<ApproveOrderQuery, Unit>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public ApproveOrderHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<Unit> Handle(ApproveOrderQuery request, CancellationToken cancellationToken)
        {
            await _orderBusinessLogic.ApproveOrderAsync(request.OrderId, request.IsExecutor);

            return Unit.Value;
        }
    }
}
