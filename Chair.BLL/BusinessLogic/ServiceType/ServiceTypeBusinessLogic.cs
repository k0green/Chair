using AutoMapper;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Repositories.ExecutorService;
using Chair.DAL.Repositories.ServiceType;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.ServiceType
{
    public class ServiceTypeBusinessLogic : IServiceTypeBusinessLogic
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IMapper _mapper;

        public ServiceTypeBusinessLogic(IServiceTypeRepository serviceTypeRepository,
            IMapper mapper)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceTypeDto>> GetAllServiceTypes()
        {
            var serviceTypes = await _serviceTypeRepository
                .GetAllAsync().ToListAsync();
            var serviceTypesDtos = _mapper.Map<List<ServiceTypeDto>>(serviceTypes);
            return serviceTypesDtos;
        }

        public async Task<ServiceTypeDto> GetServiceTypeById(Guid id)
        {
            var serviceType = await _serviceTypeRepository
                .GetByIdAsync(id);
            var serviceTypesDto = _mapper.Map<ServiceTypeDto>(serviceType);
            return serviceTypesDto;
        }

        public async Task<Guid> AddAsync(AddServiceTypeDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ServiceType>(dto);
            entity.Id = Guid.NewGuid();
            await _serviceTypeRepository.AddAsync(entity);
            await _serviceTypeRepository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(ServiceTypeDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ServiceType>(dto);
            await _serviceTypeRepository.UpdateAsync(entity);
            await _serviceTypeRepository.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _serviceTypeRepository.RemoveByIdAsync(id);
            await _serviceTypeRepository.SaveChangesAsync();
        }
    }
}
