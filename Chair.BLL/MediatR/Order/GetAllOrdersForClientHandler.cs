using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class GetAllOrdersForClientHandler : IRequestHandler<GetAllOrdersForClientQuery, List<OrderDto>>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public GetAllOrdersForClientHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersForClientQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderBusinessLogic.GetAllOrdersForClient(request.Month, request.Year);

            return result;
        }
    }
}
