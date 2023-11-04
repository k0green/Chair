using Chair.BLL.Dto.Order;
using Microsoft.AspNetCore.SignalR;

namespace Chair.BLL.BusinessLogic.Order
{
    public interface IOrderBusinessLogic
    {
        Task<List<OrderDto>> GetAllOrdersByServiceId(Guid serviceId, int month, int year);
        Task<List<OrderDto>> GetAllOrdersForClient(int month, int year);
        Task<List<OrderDto>> GetAllOrdersForExecutor(int month, int year);
        Task<List<OrderDto>> GetUnconfirmedOrdersForExecutor();
        Task<List<OrderDto>> GetUnconfirmedOrdersForClient();
        Task<OrderDto> GetOrderById(Guid id);

        Task<List<Guid>> AddManyAsync(List<AddOrderDto> dto);

        Task UpdateAsync(UpdateOrderDto dto);

        Task RemoveAsync(Guid id);

        public Task ApproveOrderAsync(Guid orderId, bool IsExecutor);

        public Task CancelOrderAsync(Guid orderId);
    }
}
