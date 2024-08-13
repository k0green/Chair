using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.Dto.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Chair.BLL.Commons;
using Chair.DAL.Repositories.Base;

namespace Chair.BLL.BusinessLogic.Order
{
    public class OrderBusinessLogic : IOrderBusinessLogic
    {
        private readonly IBaseWithManyRepository<DAL.Data.Entities.Order> _orderRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserInfo _userInfo;

        public OrderBusinessLogic(IBaseWithManyRepository<DAL.Data.Entities.Order> orderRepository,
            IHubContext<NotificationHub> hubContext,
            UserInfo userInfo,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _hubContext = hubContext;
            _mapper = mapper;
            _userInfo = userInfo;
        }

        public async Task<List<OrderDto>> GetAllOrdersByServiceId(Guid serviceId, int month, int year)
        {
            return (await GetOrderByPeriodUsePredicate(month, year, x => x.ExecutorServiceId == serviceId)).Where(x => string.IsNullOrEmpty(x.ClientId)).ToList();
        }

        public async Task<List<OrderDto>> GetAllOrdersForExecutor(int month, int year)
        {
            var userId = await _userInfo.GetUserIdFromToken();;
            return await GetOrderByPeriodUsePredicate(month, year, x => x.ExecutorService.Executor.UserId == userId);
        }

        public async Task<List<OrderDto>> GetAllOrdersForClient(int month, int year)
        {
            var userId = await _userInfo.GetUserIdFromToken();
            return await GetOrderByPeriodUsePredicate(month, year, x => x.ClientId == userId);
        }

        public async Task<UnconfirmedOrdersDto> GetUnconfirmedOrdersForExecutor()
        {
            var userId = await _userInfo.GetUserIdFromToken();
            return await GetUnconfirmedOrdersUsePredicate(x => x.ExecutorService.Executor.UserId == userId && !x.ClientApprove);
        }

        public async Task<UnconfirmedOrdersDto> GetUnconfirmedOrdersForClient()
        {
            var userId = await _userInfo.GetUserIdFromToken();
            return await GetUnconfirmedOrdersUsePredicate(x => x.ClientId == userId && !x.ExecutorApprove);
        }

        public async Task<OrderDto> GetOrderById(Guid id)
        {
            return await GetOrders(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        
        private async Task<List<OrderDto>> GetOrderByPeriodUsePredicate(int month, int year, Expression<Func<DAL.Data.Entities.Order, bool>>? predicate = null)
        {
            return await GetOrders(predicate)
                .Where(x=>x.StarDate.Year == year)
                .Where(x=>x.StarDate.Month == month)
                .OrderBy(x => x.StarDate)
                .ToListAsync();
        }
        
        private async Task<UnconfirmedOrdersDto> GetUnconfirmedOrdersUsePredicate(Expression<Func<DAL.Data.Entities.Order, bool>>? predicate = null)
        {
            var orders = await _orderRepository
                .GetAllByPredicateAsQueryable(predicate)
                .Where(x => x.StarDate >= DateTime.UtcNow.Date)
                .Select(x => new OrderDto()
                {
                    ExecutorServiceId = x.ExecutorServiceId,
                    ClientApprove = x.ClientApprove,
                    ClientComment = x.ClientComment,
                    ClientId = x.ClientId,
                    ClientName = x.User.AccountName,
                    Duration = x.Duration,
                    ExecutorApprove = x.ExecutorApprove,
                    ExecutorComment = x.ExecutorComment,
                    ExecutorName = x.ExecutorService.Executor.User.AccountName,
                    ExecutorProfileName = x.ExecutorService.Executor.Name,
                    Id = x.Id,
                    StarDate = x.StarDate,
                    Price = x.Price,
                    ServiceTypeName = x.ExecutorService.ServiceType.Name
                }).ToListAsync();
            return new UnconfirmedOrdersDto()
            {
                ByClient = orders.Where(x => x is { ExecutorApprove: true, ClientApprove: false } && !string.IsNullOrEmpty(x.ClientId)).ToList(),
                ByMaster = orders.Where(x => x is { ExecutorApprove: false, ClientApprove: false } && !string.IsNullOrEmpty(x.ClientId)).ToList(),
                ForToday = orders.Where(x => x.StarDate.Date == DateTime.UtcNow.Date).ToList(),
                ForWeek = orders.Where(x =>
                    x.StarDate >= DateTime.UtcNow.Date && x.StarDate.Date < DateTime.UtcNow.Date.AddDays(8)).ToList(),
            };
        }

        private IQueryable<OrderDto> GetOrders(
            Expression<Func<DAL.Data.Entities.Order, bool>>? predicate = null)
        {
            return _orderRepository.GetAllByPredicateAsQueryable(predicate)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider);
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
            var entity = await _orderRepository.GetByIdAsync(dto.Id);
            dto.ClientId = entity.ClientId;
            _mapper.Map(dto, entity);
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

        public async Task EnrollOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentNullException("Order doesnt exist");
            var userId = await _userInfo.GetUserIdFromToken();
            order.ClientId = userId;
            order.ClientComment = null;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
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
