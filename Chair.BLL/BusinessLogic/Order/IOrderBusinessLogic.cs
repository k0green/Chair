using Chair.BLL.Dto.Order;

namespace Chair.BLL.BusinessLogic.Order
{
    public interface IOrderBusinessLogic
    {
        Task<List<OrderDto>> GetAllOrdersByServiceId(Guid serviceId);
        Task<OrderDto> GetOrderById(Guid id);

        Task<List<Guid>> AddManyAsync(List<AddOrderDto> dto);

        Task UpdateAsync(UpdateOrderDto dto);

        Task RemoveAsync(Guid id);
    }
}
