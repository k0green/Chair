using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.Order;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Contact;
using Chair.DAL.Repositories.Order;
using Chair.DAL.Repositories.ExecutorService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Chair.BLL.Commons;

namespace Chair.BLL.BusinessLogic.Order
{
    public class OrderBusinessLogic : IOrderBusinessLogic
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IExecutorServiceRepository _executorServiceRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserInfo _userInfo;

        public OrderBusinessLogic(IOrderRepository orderRepository,
            IExecutorServiceRepository executorServiceRepository,
            IContactRepository contactRepository,
            IHubContext<NotificationHub> hubContext,
            UserInfo userInfo,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _executorServiceRepository = executorServiceRepository;
            _contactRepository = contactRepository;
            _hubContext = hubContext;
            _mapper = mapper;
            _userInfo = userInfo;
        }

        public async Task<List<OrderDto>> GetAllOrdersByServiceId(Guid serviceId)
        {
            var orders = await _orderRepository.GetAllByPredicateAsQueryable(x => x.ExecutorServiceId == serviceId)
                .Include(x => x.User)
                .Include(x => x.ExecutorService)
                .Include(x => x.ExecutorService.Executor)
                .Include(x => x.ExecutorService.Executor.User)
                .Include(x => x.ExecutorService.ServiceType)
                .ToListAsync();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return orderDtos;
        }

        public async Task<OrderDto> GetOrderById(Guid id)
        {
            var order = await _orderRepository.GetAllByPredicateAsQueryable(x => x.Id == id)
                .Include(x => x.User)
                .Include(x => x.ExecutorService)
                .Include(x => x.ExecutorService.Executor)
                .Include(x => x.ExecutorService.Executor.User)
                .Include(x => x.ExecutorService.ServiceType)
                .FirstOrDefaultAsync();

            var orderDto = _mapper.Map<OrderDto>(order);

            return orderDto;
        }

        public async Task<List<Guid>> AddManyAsync(List<AddOrderDto> dtos)
        {
            var entities = _mapper.Map<List<DAL.Data.Entities.Order>>(dtos);
            entities.ForEach(x=>x.Id = Guid.NewGuid());

            await _orderRepository.AddManyAsync(entities);
            await _orderRepository.SaveChangesAsync();

            return entities.Select(x=>x.Id).ToList();
        }

        public async Task UpdateAsync(UpdateOrderDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.Order>(dto);

            await _orderRepository.UpdateAsync(entity);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _orderRepository.RemoveByIdAsync(id);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task ApproveOrderAsync(Guid orderId, bool IsExecutor)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentNullException("Order doesnt exist");
            string userId;
            switch (IsExecutor)
            {
                case true:
                    order.ExecutorApprove = true;
                    userId = order.ClientId;
                    break;
                case false:
                    order.ClientApprove = true;
                    userId = order.ExecutorService.Executor.UserId;
                    break;
            }

            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
            await _hubContext.Clients.User(userId).SendAsync("ReceiveOrderNotification", "Заказ подтвержден");
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentNullException("Order doesnt exist");
            order.ClientApprove = false;
            order.ExecutorApprove = false;
            order.ClientId = null;
            order.ClientComment = null;
            await _orderRepository.UpdateAsync(order);

            await _orderRepository.SaveChangesAsync();
            await _hubContext.Clients.Users(new List<string>() { order.ClientId, order.ExecutorService.Executor.UserId }).SendAsync("ReceiveOrderNotification", "Заказ подтвержден");
        }
    }
}
