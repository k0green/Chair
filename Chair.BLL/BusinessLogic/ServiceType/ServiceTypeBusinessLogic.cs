using AutoMapper;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Extension;
using Chair.DAL.Extension.Models;
using Chair.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.ServiceType
{
    public class ServiceTypeBusinessLogic : IServiceTypeBusinessLogic
    {
        private readonly IBaseRepository<DAL.Data.Entities.ServiceType> _serviceTypeRepository;
        private readonly IMapper _mapper;

        public ServiceTypeBusinessLogic(IBaseRepository<DAL.Data.Entities.ServiceType> serviceTypeRepository,
            IMapper mapper)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceTypeDto>> GetAllServiceTypes(Guid? parentId)
        {
            var serviceTypes = await _serviceTypeRepository
                .GetAllAsync()
                //.Where(x => x.ExecutorServices.ToList().Count > 0)
                .WhereIf(parentId != null, x => x.ParentId == (Guid)parentId)
                .WhereIf(parentId == null, x => x.ParentId == null)
                .OrderByDescending(x => x.ExecutorServices.ToList().Count)
                .ToListAsync();
            var serviceTypesDtos = _mapper.Map<List<ServiceTypeDto>>(serviceTypes);
            return serviceTypesDtos;
        }

        public async Task<List<ServiceTypeDto>> GetPopularServiceTypes(FilterModel filter)
        {
            var serviceTypes = await _serviceTypeRepository
                .GetAllAsync()
                .Where(x => x.ExecutorServices.ToList().Count > 0)
                .Where(x => x.ParentId != null)
                .OrderByDescending(x => x.ExecutorServices.ToList().Count)
                .Select(x => new ServiceTypeDto
                {
                    Id = (Guid)x.ParentId,
                    Name = x.Parent.Name,
                    Icon = x.Parent.Icon,
                })
                .ToFilterView(filter).ToListAsync();
            return serviceTypes;
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
