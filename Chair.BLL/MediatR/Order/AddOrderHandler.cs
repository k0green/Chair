using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class AddOrderHandler : IRequestHandler<AddOrderQuery, List<Guid>>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public AddOrderHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<List<Guid>> Handle(AddOrderQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderBusinessLogic.AddManyAsync(request.AddOrderDtos);

            return result;
        }
    }
}
