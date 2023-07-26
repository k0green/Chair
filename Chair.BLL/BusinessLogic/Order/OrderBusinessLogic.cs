using AutoMapper;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.Order;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Contact;
using Chair.DAL.Repositories.Order;
using Chair.DAL.Repositories.ExecutorService;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.Order
{
    public class OrderBusinessLogic : IOrderBusinessLogic
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IExecutorServiceRepository _executorServiceRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public OrderBusinessLogic(IOrderRepository orderRepository,
            IExecutorServiceRepository executorServiceRepository,
            IContactRepository contactRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _executorServiceRepository = executorServiceRepository;
            _contactRepository = contactRepository;
            _mapper = mapper;
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
    }
}
