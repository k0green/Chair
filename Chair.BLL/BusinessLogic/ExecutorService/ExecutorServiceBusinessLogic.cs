using System.Linq.Expressions;
using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.Minio;
using Chair.BLL.Dto.Order;
using Chair.DAL.Data.Entities;
using Chair.DAL.Extension;
using Chair.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using opr_lib;
using ExecutorServiceDao = Chair.DAL.Data.Entities.ExecutorService;

namespace Chair.BLL.BusinessLogic.ExecutorService
{
    public class ExecutorServiceBusinessLogic : IExecutorServiceBusinessLogic
    {
        private readonly IBaseWithManyRepository<ProductFile<ExecutorServiceDao>> _fileRepository;
        private readonly IBaseRepository<ExecutorServiceDao> _executorServiceRepository;
        private readonly IBaseWithManyRepository<DAL.Data.Entities.Order> _orderRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;

        public ExecutorServiceBusinessLogic(IBaseRepository<ExecutorServiceDao> executorServiceRepository,
            IBaseWithManyRepository<ProductFile<ExecutorServiceDao>> fileRepository,
            IBaseWithManyRepository<DAL.Data.Entities.Order> orderRepository,
            UserInfo userInfo,
            IMapper mapper)
        {
            _executorServiceRepository = executorServiceRepository;
            _orderRepository = orderRepository;
            _fileRepository = fileRepository;
            _userInfo = userInfo;
            _mapper = mapper;
        }

        public async Task<List<ExecutorServiceDto>> GetAllServicesByExecutorId(Guid executorId)
        {
            var executorServiceDtos = _executorServiceRepository
                .GetAllByPredicateAsQueryable(x => x.ExecutorId == executorId)
                .Select(x => new ExecutorServiceDto
                {
                    Id = x.Id,
                    Place = new Place()
                    {
                        Address  = x.Address,
                        Position = new Position()
                        {
                            Lng = x.Lng,
                            Lat = x.Lat,
                        }
                    },
                    Orders = x.Orders.Any() ? x.Orders.Select(o => new OrderDto()
                    {
                        Id = o.Id,
                        StarDate = o.StarDate,
                        ClientId = o.ClientId,
                    }).ToList() : new List<OrderDto>(),
                    Description = x.Description,
                    Duration = x.Duration,
                    ExecutorId = x.ExecutorId,
                    ExecutorName = x.Executor.Name,
                    Price = x.Price,
                    Rating = x.Reviews.Any() ? (decimal)x.Reviews.Average(r => r.Stars) : 5,
                    Photos = x.Images.Select(i => new ShortMinioFileDto()
                    {
                        Id = i.Id,
                        Url = i.MinioFile.Url
                    }).ToList(),
                    ServiceTypeId = x.ServiceTypeId,
                    ServiceTypeName = x.ServiceType.Name,
                }).ToList();
            return executorServiceDtos;
        }
        
        public async Task<(List<GroupExecutorServiceDto>, int)> GetAllServices(FilterModelWithPeriods filter)
        {
            return await GetAllServicesByPredicate(filter: filter);
        }
        
        public async Task<ExecutorServiceDto> GetOptimizeService(FilterModelWithPeriods filter, Guid serviceTypeId, List<Condition> conditions)
        {
            Dictionary<int, Guid> myDictionary = new Dictionary<int, Guid>();
            var items = (await GetAllServicesByPredicate(x => x.ServiceTypeId == serviceTypeId, filter: filter)).Item1.First();
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
        
        public async Task<(List<GroupExecutorServiceDto>, int)> GetAllServicesByTypeId(Guid serviceTypeId, FilterModelWithPeriods filter)
        {
            return await GetAllServicesByPredicate(x => x.ServiceTypeId == serviceTypeId, filter: filter);
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

        private async Task<(List<GroupExecutorServiceDto>, int)> GetAllServicesByPredicate(Expression<Func<DAL.Data.Entities.ExecutorService, bool>>? predicate = null, FilterModelWithPeriods? filter = null)
        {
            var executorServiceDtos = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(predicate)
                .Select(x => new ExecutorServiceDto
                {
                    Id = x.Id,
                    Place = new Place()
                    {
                      Address  = x.Address,
                      Position = new Position()
                      {
                          Lng = x.Lng,
                          Lat = x.Lat,
                      }
                    },
                    Orders = x.Orders.Any() ? x.Orders.Select(o => new OrderDto()
                    {
                        Id = o.Id,
                        StarDate = o.StarDate,
                        ClientId = o.ClientId,
                    }).ToList() : new List<OrderDto>(),
                    Description = x.Description,
                    Duration = x.Duration,
                    ExecutorId = x.ExecutorId,
                    ExecutorName = x.Executor.Name,
                    Price = x.Price,
                    Rating = x.Reviews.Any() ? (decimal)x.Reviews.Average(r => r.Stars) : 5,
                    Photos = x.Images.Select(i => new ShortMinioFileDto()
                    {
                        Id = i.Id,
                        Url = i.MinioFile.Url
                    }).ToList(),
                    ServiceTypeId = x.ServiceTypeId,
                    ServiceTypeName = x.ServiceType.Name,
                }).ToListAsync();

            if (filter != null)
            {
                if (filter is { Times: not null, Dates: not null } && (filter.Dates.Any() || filter.Times.Any()))
                {
                    executorServiceDtos = executorServiceDtos
                        .Where(x =>
                            x.Orders.Any(o =>
                                (filter.Dates.Any() && filter.Dates.Select(d => d.Date).Contains(o.StarDate.Date)) ||
                                (filter.Times.Any() && filter.Times.Any(t => o.StarDate.TimeOfDay >= t.StartTime.TimeOfDay && o.StarDate <= t.EndTime))
                            )
                        )
                        .ToList();
                }
            }

            var totalCount = executorServiceDtos.Count;
            /*if (filter?.Filter != null && filter.Filter.Filters != null)*/
            executorServiceDtos = executorServiceDtos.AsQueryable().ToFilterView(filter, out totalCount).ToList();

            var groupedServices = executorServiceDtos
                .GroupBy(x => new { x.ServiceTypeId, x.ServiceTypeName })
                .Select(group => new GroupExecutorServiceDto
                {
                    Id = group.Key.ServiceTypeId,
                    ServiceTypeName = group.Key.ServiceTypeName,
                    Services = group.ToList()
                })
                .ToList();
            
            return (groupedServices, totalCount);
        }

        public async Task<ExecutorServiceDto> GetExecutorServiceById(Guid id)
        {
            var executorServiceDto = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(x=>x.Id == id)
                .Select(x => new ExecutorServiceDto
                {
                    Id = x.Id,
                    Place = new Place()
                    {
                        Address  = x.Address,
                        Position = new Position()
                        {
                            Lng = x.Lng,
                            Lat = x.Lat,
                        }
                    },
                    Orders = x.Orders.Any() ? x.Orders.Select(o => new OrderDto()
                    {
                        Id = o.Id,
                        StarDate = o.StarDate,
                        ClientId = o.ClientId
                    }).ToList() : new List<OrderDto>(),
                    Description = x.Description,
                    Duration = x.Duration,
                    ExecutorId = x.ExecutorId,
                    ExecutorName = x.Executor.Name,
                    Price = x.Price,
                    Rating = x.Reviews.Any() ? (decimal)x.Reviews.Average(r => r.Stars) : 5,
                    Photos = x.Images.Select(i => new ShortMinioFileDto()
                    {
                        Id = i.Id,
                        Url = i.MinioFile.Url
                    }).ToList(),
                    ServiceTypeId = x.ServiceTypeId,
                    ServiceTypeName = x.ServiceType.Name,
                }).FirstOrDefaultAsync();
            return executorServiceDto;
        }

        public async Task<Guid> AddAsync(AddExecutorServiceDto dto)
        {
            var existingEntity = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(x => x.ExecutorId == dto.ExecutorId)
                .FirstOrDefaultAsync(x => x.ServiceTypeId == dto.ServiceTypeId);
            if (existingEntity != null)
            {
                await UpdateAsync(new UpdateExecutorServiceDto
                {
                    Description = dto.Description,
                    Duration = dto.Duration,
                    ExecutorId = dto.ExecutorId,
                    Id = existingEntity.Id,
                    PhotoIds = dto.PhotoIds,
                    Place = dto.Place,
                    Price = dto.Price,
                    ServiceTypeId = dto.ServiceTypeId,
                });
            }
            else
            {
                var entity = _mapper.Map<DAL.Data.Entities.ExecutorService>(dto);
                entity.Id = Guid.NewGuid();
                await _executorServiceRepository.AddAsync(entity);
                await AddPhotos(dto.PhotoIds, entity.Id);
                await _executorServiceRepository.SaveChangesAsync();   
                return entity.Id;
            }
            return existingEntity.Id;
        }

        public async Task UpdateAsync(UpdateExecutorServiceDto dto)
        {
            var entity = await _executorServiceRepository.GetByIdAsync(dto.Id);
            _mapper.Map(dto, entity);
            await _executorServiceRepository.UpdateAsync(entity);
            var photoIds = await _fileRepository
                .GetAllByPredicateAsQueryable(x => x.ProductId == entity.Id)
                .Select(x=>x.Id)
                .ToListAsync();
            await _fileRepository.RemoveManyByIdsAsync(photoIds);
            await AddPhotos(dto.PhotoIds, entity.Id);
            await _executorServiceRepository.SaveChangesAsync();
            await _fileRepository.SaveChangesAsync();
        }

        private async Task AddPhotos(IEnumerable<Guid> photoIds, Guid entityId)
        {
            foreach (var id in photoIds)
            {
                await _fileRepository.AddAsync(new ProductFile<ExecutorServiceDao>()
                {
                    Id = Guid.NewGuid(),
                    ProductId = entityId,
                    MinioFileId = id
                });
            }

            await _fileRepository.SaveChangesAsync();
        }


        public async Task RemoveAsync(Guid id)
        {
            var entity = await _executorServiceRepository.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Card not found");
            var orders = _orderRepository.GetAllByPredicateAsQueryable(x => x.ExecutorServiceId == id)
                .Any(x => x.StarDate >= DateTime.Now && x.ClientId != null);
            if (orders)
                throw new Exception("Card has orders");
            entity.IsDeleted = true;
            await _executorServiceRepository.UpdateAsync(entity);
            await _executorServiceRepository.SaveChangesAsync();
        }
    }
}
