using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderQuery, Unit>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public UpdateOrderHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<Unit> Handle(UpdateOrderQuery request, CancellationToken cancellationToken)
        {
            await _orderBusinessLogic.UpdateAsync(request.UpdateOrderDto);

            return Unit.Value;
        }
    }
}
