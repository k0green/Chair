using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class RemoveOrderHandler : IRequestHandler<RemoveOrderQuery, Unit>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public RemoveOrderHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<Unit> Handle(RemoveOrderQuery request, CancellationToken cancellationToken)
        {
            await _orderBusinessLogic.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
