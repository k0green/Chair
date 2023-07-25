using AutoMapper;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.ExecutorService;
using Chair.DAL.Repositories.Image;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.ExecutorService
{
    public class ExecutorServiceBusinessLogic : IExecutorServiceBusinessLogic
    {
        private readonly IExecutorServiceRepository _executorServiceRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public ExecutorServiceBusinessLogic(IExecutorServiceRepository executorServiceRepository,
            IImageRepository imageRepository,
            IMapper mapper)
        {
            _executorServiceRepository = executorServiceRepository;
            _imageRepository = imageRepository;
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
