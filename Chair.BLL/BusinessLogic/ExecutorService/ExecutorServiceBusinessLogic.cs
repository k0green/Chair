using System.Linq.Expressions;
using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Data.Entities;
using Chair.DAL.Extension;
using Chair.DAL.Extension.Models;
using Chair.DAL.Repositories.ExecutorService;
using Chair.DAL.Repositories.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using opr_lib;

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
        
        public async Task<List<GroupExecutorServiceDto>> GetAllServices(FilterModelWithPeriods filter)
        {
            return await GetAllServicesByPredicate(filter: filter);
        }
        
        public async Task<ExecutorServiceDto> GetOptimizeService(FilterModelWithPeriods filter, Guid serviceTypeId, List<Condition> conditions)
        {
            Dictionary<int, Guid> myDictionary = new Dictionary<int, Guid>();
            var items = (await GetAllServicesByPredicate(x => x.ServiceTypeId == serviceTypeId, filter: filter)).First();
            var matrix = new decimal?[items.Services.Count(), 4];
            var i = 0;
            foreach (var item in items.Services)
            {
                matrix[i, 0] = item.AvailableSlots;
                matrix[i, 1] = item.Price;
                matrix[i, 2] = item.Rating;
                matrix[i, 3] = item.Duration.Hour * 60 + item.Duration.Minute;
                
                myDictionary.Add(i, item.Id);

                i++;
            }

            var resultId = Optimization.GetResult(matrix, conditions, onMax: true, myDictionary);
            return items.Services.First(x => x.Id == resultId);
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
        
        public async Task<List<GroupExecutorServiceDto>> GetAllServicesByPredicate(Expression<Func<DAL.Data.Entities.ExecutorService, bool>>? predicate = null, FilterModelWithPeriods? filter = null)
        {
            var executorServices = _executorServiceRepository
                .GetAllByPredicateAsQueryable(predicate)
                .Include(x => x.Images)
                .Include(x => x.ServiceType)
                .Include(x => x.Executor)
                .Include(x => x.Reviews)
                .Include(x => x.Orders.Where(y => y.ClientId == null))
                .ToList();

            if (filter != null)
            {
                if (filter is { Times: not null, Dates: not null } && (filter.Dates.Any() || filter.Times.Any()))
                {
                    executorServices = executorServices
                        .Where(x =>
                            x.Orders.Any(o =>
                                (filter.Dates.Any() && filter.Dates.Select(d => d.Date).Contains(o.StarDate.Date)) ||
                                (filter.Times.Any() && filter.Times.Any(t => o.StarDate.TimeOfDay >= t.StartTime.TimeOfDay && o.StarDate <= t.EndTime))
                            )
                        )
                        .ToList();
                }
            }

            var executorServiceDtos = _mapper.Map<List<ExecutorServiceDto>>(executorServices);

            if (filter?.Filter != null)
                executorServiceDtos = executorServiceDtos.AsQueryable().ToFilterView(filter.Filter).ToList();

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
