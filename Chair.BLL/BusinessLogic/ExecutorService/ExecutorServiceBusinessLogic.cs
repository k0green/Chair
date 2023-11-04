using System.Linq.Expressions;
using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.ExecutorService;
using Chair.DAL.Repositories.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Chair.BLL.BusinessLogic.ExecutorService
{
    public class ExecutorServiceBusinessLogic : IExecutorServiceBusinessLogic
    {
        private readonly IExecutorServiceRepository _executorServiceRepository;
        private readonly IImageRepository _imageRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;

        public ExecutorServiceBusinessLogic(IExecutorServiceRepository executorServiceRepository,
            IImageRepository imageRepository,
            UserInfo userInfo,
            IMapper mapper)
        {
            _executorServiceRepository = executorServiceRepository;
            _imageRepository = imageRepository;
            _userInfo = userInfo;
            _mapper = mapper;
        }

        public async Task<List<ExecutorServiceDto>> GetAllServicesByExecutorId(Guid executorId)
        {
            var executorServices = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(x => x.ExecutorId == executorId)
                .Include(x=>x.Images)
                .ToListAsync();
            var executorServiceDtos = _mapper.Map<List<ExecutorServiceDto>>(executorServices);
            return executorServiceDtos;
        }
        
        public async Task<List<GroupExecutorServiceDto>> GetAllServices()
        {
            /*var executorServices = await _executorServiceRepository
                .GetAllByPredicateAsQueryable()
                .Include(x => x.Images)
                .Include(x => x.ServiceType)
                .Include(x => x.Executor)
                .Include(x => x.Reviews)
                .Include(x => x.Orders.Where(y => y.ClientId == null))
                .ToListAsync();
            
            var executorServiceDtos = _mapper.Map<List<ExecutorServiceDto>>(executorServices);

            var groupedServices = executorServiceDtos
                .GroupBy(x => new { x.ServiceTypeId, x.ServiceTypeName })
                .Select(group => new GroupExecutorServiceDto
                {
                    Id = group.Key.ServiceTypeId,
                    ServiceTypeName = group.Key.ServiceTypeName,
                    Services = group.ToList()
                })
                .ToList();

            return groupedServices;*/
            return await GetAllServicesByPredicate();
        }
        
        public async Task<List<GroupExecutorServiceDto>> GetAllServicesByTypeId(Guid serviceTypeId)
        {
            return await GetAllServicesByPredicate(x => x.ServiceTypeId == serviceTypeId);
        }
        
        public async Task<List<LookupDto>> GetAllServicesNamesByUserId()
        {
            var userId = await _userInfo.GetUserIdFromToken();
            return await _executorServiceRepository
                .GetAllByPredicateAsQueryable(x=>x.Executor.UserId == userId)
                .Select(x=> new LookupDto()
                {
                    Id = x.Id,
                    Name = x.ServiceType.Name
                }).ToListAsync();
        }
        
        public async Task<List<LookupDto>> GetAllServicesNames()
        {
            return await _executorServiceRepository
                .GetAllByPredicateAsQueryable()
                .Select(x=> new LookupDto()
                {
                    Id = x.Id,
                    Name = x.ServiceType.Name
                }).ToListAsync();
        }
        
        public async Task<List<GroupExecutorServiceDto>> GetAllServicesByPredicate(Expression<Func<DAL.Data.Entities.ExecutorService, bool>>? predicate = null)
        {
            var executorServices = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(predicate)
                .Include(x => x.Images)
                .Include(x => x.ServiceType)
                .Include(x => x.Executor)
                .Include(x => x.Reviews)
                .Include(x => x.Orders.Where(y => y.ClientId == null))
                .ToListAsync();
            
            var executorServiceDtos = _mapper.Map<List<ExecutorServiceDto>>(executorServices);

            var groupedServices = executorServiceDtos
                .GroupBy(x => new { x.ServiceTypeId, x.ServiceTypeName })
                .Select(group => new GroupExecutorServiceDto
                {
                    Id = group.Key.ServiceTypeId,
                    ServiceTypeName = group.Key.ServiceTypeName,
                    Services = group.ToList()
                })
                .ToList();

            return groupedServices;
        }

        public async Task<ExecutorServiceDto> GetExecutorServiceById(Guid id)
        {
            var executorService = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(x=>x.Id == id)
                .Include(x=>x.Images)
                .FirstOrDefaultAsync();
            var executorServiceDto = _mapper.Map<ExecutorServiceDto>(executorService);
            return executorServiceDto;
        }

        public async Task<Guid> AddAsync(AddExecutorServiceDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ExecutorService>(dto);
            entity.Id = Guid.NewGuid();
            await _executorServiceRepository.AddAsync(entity);
            foreach (var url in dto.ImageURLs)
            {
                await _imageRepository.AddAsync(new Image()
                {
                    Id = Guid.NewGuid(),
                    URL = url,
                    ObjectId = entity.Id,
                });
            }
            await _executorServiceRepository.SaveChangesAsync();
            await _imageRepository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(UpdateExecutorServiceDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ExecutorService>(dto);
            await _executorServiceRepository.UpdateAsync(entity);
            var imageIds = await _imageRepository.GetAllByPredicateAsQueryable(x => x.ObjectId == entity.Id).Select(x=>x.Id).ToListAsync();
            await _imageRepository.RemoveManyByIdsAsync(imageIds);
            foreach (var url in dto.ImageURLs)
            {
                await _imageRepository.AddAsync(new Image()
                {
                    Id = Guid.NewGuid(),
                    URL = url,
                    ObjectId = entity.Id,
                });
            }
            await _executorServiceRepository.SaveChangesAsync();
            await _imageRepository.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _executorServiceRepository.RemoveByIdAsync(id);
            await _executorServiceRepository.SaveChangesAsync();
        }
    }
}
