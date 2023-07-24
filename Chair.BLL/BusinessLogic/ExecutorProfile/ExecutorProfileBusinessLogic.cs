using AutoMapper;
using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Repositories.ExecutorService;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.ExecutorProfile
{
    public class ExecutorProfileBusinessLogic : IExecutorProfileBusinessLogic
    {
        private readonly IExecutorProfileRepository _executorProfileRepository;
        private readonly IExecutorServiceRepository _executorServiceRepository;
        private readonly IMapper _mapper;

        public ExecutorProfileBusinessLogic(IExecutorProfileRepository executorProfileRepository,
            IExecutorServiceRepository executorServiceRepository,
            IMapper mapper)
        {
            _executorProfileRepository = executorProfileRepository;
            _executorServiceRepository = executorServiceRepository;
            _mapper = mapper;
        }

        public async Task<List<ExecutorProfileDto>> GetAllProfilesByServiceTypeId(Guid serviceTypeId)
        {
            var executorServicesIds = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(x => x.ServiceTypeId == serviceTypeId).Select(x=>x.Id).ToListAsync();
            var executorProfiles = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(x => executorServicesIds.Contains(x.ExecutorId)).ToListAsync();
            var executorProfilesDtos = _mapper.Map<List<ExecutorProfileDto>>(executorProfiles);
            return executorProfilesDtos;
        }

        public async Task<ExecutorProfileDto> GetExecutorProfileById(Guid id)
        {
            var executorProfile = await _executorProfileRepository.GetAllByPredicateAsQueryable(x => x.Id == id)
                .Include(x => x.User).FirstOrDefaultAsync();
            var executorProfileDto = _mapper.Map<ExecutorProfileDto>(executorProfile);
            return executorProfileDto;
        }

        public async Task<Guid> AddAsync(AddExecutorProfileDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ExecutorProfile>(dto);
            entity.Id = Guid.NewGuid();
            await _executorProfileRepository.AddAsync(entity);
            await _executorProfileRepository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(UpdateExecutorProfileDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ExecutorProfile>(dto);
            await _executorProfileRepository.UpdateAsync(entity);
            await _executorProfileRepository.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _executorProfileRepository.RemoveByIdAsync(id);
            await _executorProfileRepository.SaveChangesAsync();
        }
    }
}
