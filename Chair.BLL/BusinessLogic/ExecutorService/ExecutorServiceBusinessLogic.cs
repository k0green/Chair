using AutoMapper;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Repositories.ExecutorService;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.ExecutorService
{
    public class ExecutorServiceBusinessLogic : IExecutorServiceBusinessLogic
    {
        private readonly IExecutorServiceRepository _executorServiceRepository;
        private readonly IMapper _mapper;

        public ExecutorServiceBusinessLogic(IExecutorServiceRepository executorServiceRepository,
            IMapper mapper)
        {
            _executorServiceRepository = executorServiceRepository;
            _mapper = mapper;
        }

        public async Task<List<ExecutorServiceDto>> GetAllServicesByExecutorId(Guid executorId)
        {
            var executorServices = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(x => x.ExecutorId == executorId).ToListAsync();
            var executorServiceDtos = _mapper.Map<List<ExecutorServiceDto>>(executorServices);
            return executorServiceDtos;
        }

        public async Task<ExecutorServiceDto> GetExecutorServiceById(Guid id)
        {
            var executorService = await _executorServiceRepository
                .GetByIdAsync(id);
            var executorServiceDto = _mapper.Map<ExecutorServiceDto>(executorService);
            return executorServiceDto;
        }

        public async Task<Guid> AddAsync(AddExecutorServiceDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ExecutorService>(dto);
            entity.Id = Guid.NewGuid();
            await _executorServiceRepository.AddAsync(entity);
            await _executorServiceRepository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(UpdateExecutorServiceDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ExecutorService>(dto);
            await _executorServiceRepository.UpdateAsync(entity);
            await _executorServiceRepository.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _executorServiceRepository.RemoveByIdAsync(id);
            await _executorServiceRepository.SaveChangesAsync();
        }
    }
}
