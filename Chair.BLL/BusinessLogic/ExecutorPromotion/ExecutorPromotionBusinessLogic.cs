using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.ExecutorPromotion;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.Minio;
using Chair.BLL.Dto.Order;
using Chair.DAL.Data.Entities;
using Chair.DAL.Extension;
using Chair.DAL.Extension.Models;
using Chair.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using opr_lib;
using ExecutorPromotionDao = Chair.DAL.Data.Entities.ExecutorPromotion;

namespace Chair.BLL.BusinessLogic.ExecutorPromotion
{
    public class ExecutorPromotionBusinessLogic : IExecutorPromotionBusinessLogic
    {
        private readonly IBaseWithManyRepository<ProductFile<ExecutorPromotionDao>> _fileRepository;
        private readonly IBaseRepository<ExecutorPromotionDao> _executorPromotionRepository;
        private readonly IMapper _mapper;

        public ExecutorPromotionBusinessLogic(IBaseRepository<ExecutorPromotionDao> executorPromotionRepository,
            IBaseWithManyRepository<ProductFile<ExecutorPromotionDao>> fileRepository,
            IMapper mapper)
        {
            _executorPromotionRepository = executorPromotionRepository;
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<List<ExecutorPromotionDto>> GetAllPromotionsByExecutorId(Guid executorId)
        {
            var executorServiceDtos = _executorPromotionRepository
                .GetAllByPredicateAsQueryable(x => x.ExecutorId == executorId)
                .Where(x => !x.IsDeleted)
                .Select(x => new ExecutorPromotionDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    ExecutorId = x.ExecutorId,
                    ExecutorName = x.Executor.Name,
                    Photos = x.Images.Select(i => new ShortMinioFileDto()
                    {
                        Id = i.Id,
                        Url = i.MinioFile.Url
                    }).ToList(),
                }).ToList();
            return executorServiceDtos;
        }

        public async Task<List<ExecutorPromotionDto>> GetAllPromotions(FilterModel filter)
        {
            var executorServiceDtos = _executorPromotionRepository
                .GetAllByPredicateAsQueryable()
                .Where(x => !x.IsDeleted)
                .Select(x => new ExecutorPromotionDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    ExecutorId = x.ExecutorId,
                    ExecutorName = x.Executor.Name,
                    Photos = x.Images.Select(i => new ShortMinioFileDto()
                    {
                        Id = i.Id,
                        Url = i.MinioFile.Url
                    }).ToList(),
                }).ToList();
            return executorServiceDtos;
        }

        public async Task<ExecutorPromotionDto> GetByIdPromotion(Guid id)
        {
            var executorServiceDto = _executorPromotionRepository
                .GetAllByPredicateAsQueryable(x => x.Id == id)
                .Select(x => new ExecutorPromotionDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    ExecutorId = x.ExecutorId,
                    ExecutorName = x.Executor.Name,
                    Photos = x.Images.Select(i => new ShortMinioFileDto()
                    {
                        Id = i.Id,
                        Url = i.MinioFile.Url
                    }).ToList(),
                }).First();
            return executorServiceDto;
        }

        public async Task<Guid> AddAsync(AddExecutorPromotionDto dto)
        {
            var existingEntity = await _executorPromotionRepository
                .GetAllByPredicateAsQueryable()
                .FirstOrDefaultAsync(x => x.ExecutorId == dto.ExecutorId);
            if (existingEntity != null)
            {
                await UpdateAsync(new UpdateExecutorPromotionDto
                {
                    Description = dto.Description,
                    ExecutorId = dto.ExecutorId,
                    Id = existingEntity.Id,
                    PhotoIds = dto.PhotoIds,
                });
            }
            else
            {
                var entity = _mapper.Map<DAL.Data.Entities.ExecutorPromotion>(dto);
                entity.Id = Guid.NewGuid();
                await _executorPromotionRepository.AddAsync(entity);
                await AddPhotos(dto.PhotoIds, entity.Id);
                await _executorPromotionRepository.SaveChangesAsync();  
                await _fileRepository.SaveChangesAsync();
                return entity.Id;
            }

            return existingEntity.Id;
        }

        public async Task UpdateAsync(UpdateExecutorPromotionDto dto)
        {
            var entity = await _executorPromotionRepository.GetByIdAsync(dto.Id);
            _mapper.Map(dto, entity);
            await _executorPromotionRepository.UpdateAsync(entity);
            var photoIds = await _fileRepository
                .GetAllByPredicateAsQueryable(x => x.ProductId == entity.Id)
                .Select(x=>x.Id)
                .ToListAsync();
            if (dto.RemovePhotoIds.Any())
            {
                var deletePhotos = await _fileRepository
                    .GetAllByPredicateAsQueryable(x => dto.RemovePhotoIds.Contains(x.Id))
                    .ToListAsync();
                await _fileRepository.RemoveManyAsync(deletePhotos);   
            }
            await AddPhotos(dto.PhotoIds.Except(photoIds), entity.Id);
            await _executorPromotionRepository.SaveChangesAsync();
            await _fileRepository.SaveChangesAsync();
        }

        private async Task AddPhotos(IEnumerable<Guid> photoIds, Guid entityId)
        {
            if (photoIds != null && photoIds.Any())
            {
                foreach (var id in photoIds)
                {
                    await _fileRepository.AddAsync(new ProductFile<ExecutorPromotionDao>()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = entityId,
                        MinioFileId = id
                    });
                }
            }
        }


        public async Task RemoveAsync(Guid id)
        {
            var entity = await _executorPromotionRepository.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Card not found");
            entity.IsDeleted = true;
            await _executorPromotionRepository.UpdateAsync(entity);
            await _executorPromotionRepository.SaveChangesAsync();
        }
    }
}
