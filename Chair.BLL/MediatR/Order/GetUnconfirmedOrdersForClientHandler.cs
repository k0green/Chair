using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.CQRS.Order;
using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.MediatR.Order
{
    public class GetUnconfirmedOrdersForClientHandler : IRequestHandler<GetUnconfirmedOrdersForClientQuery, UnconfirmedOrdersDto>
    {
        private readonly IOrderBusinessLogic _orderBusinessLogic;

        public GetUnconfirmedOrdersForClientHandler(IOrderBusinessLogic orderBusinessLogic)
        {
            _orderBusinessLogic = orderBusinessLogic;
        }

        public async Task<UnconfirmedOrdersDto> Handle(GetUnconfirmedOrdersForClientQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderBusinessLogic.GetUnconfirmedOrdersForClient();

            return result;
        }
    }
}
