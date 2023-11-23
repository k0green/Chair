using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class EnrollOrderHandler : IRequestHandler<EnrollOrderQuery, Unit>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public EnrollOrderHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<Unit> Handle(EnrollOrderQuery request, CancellationToken cancellationToken)
        {
            await _orderBusinessLogic.EnrollOrderAsync(request.OrderId);

            return Unit.Value;
        }
    }
}
