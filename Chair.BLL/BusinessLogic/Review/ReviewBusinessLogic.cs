using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chair.BLL.Dto.Review;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.Review
{
    public class ReviewBusinessLogic : IReviewBusinessLogic
    {
        private readonly IBaseWithManyRepository<DAL.Data.Entities.Review> _reviewRepository;
        private readonly IBaseWithManyRepository<ProductFile<DAL.Data.Entities.Review>> _fileRepository;
        private readonly IMapper _mapper;

        public ReviewBusinessLogic(IBaseWithManyRepository<DAL.Data.Entities.Review> reviewRepository,
            IBaseWithManyRepository<ProductFile<DAL.Data.Entities.Review>> fileRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<List<ReviewDto>> GetAllReviewsForService(Guid executorServiceId)
        {
            var allReviews = await _reviewRepository
                .GetAllByPredicateAsQueryable(x => x.ExecutorServiceId == executorServiceId)
                .Include(x=>x.User)
                .Include(x=>x.Images)
                .OrderByDescending(x => x.CreateDate)
                .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var parentReviews = allReviews.Where(x => x.ParentId == null).ToList();

            foreach (var pR in parentReviews)
            {
                pR.Child = _mapper.Map<List<ReviewDto>>(allReviews.Where(x => x.ParentId == pR.Id));
            }

            return parentReviews;
        }

        public async Task<Guid> AddAsync(AddReviewDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.Review>(dto);
            entity.Id = Guid.NewGuid();
            entity.CreateDate = DateTime.Now;

            await _reviewRepository.AddAsync(entity);
            await AddPhotos(dto.PhotoIds, entity.Id);
            await _reviewRepository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(UpdateReviewDto dto)
        {
            var entity = await _reviewRepository.GetByIdAsync(dto.Id);
            _mapper.Map(dto, entity);
            await _reviewRepository.UpdateAsync(entity);
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
            await _reviewRepository.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _reviewRepository.RemoveByIdAsync(id);
            await _reviewRepository.RemoveManyByIdsAsync(await _reviewRepository
                .GetAllByPredicateAsQueryable(x => x.ParentId == id)
                .Select(x => x.Id)
                .ToListAsync());
            await _reviewRepository.SaveChangesAsync();
        }
        
        private async Task AddPhotos(IEnumerable<Guid> photoIds, Guid entityId)
        {
            if (photoIds != null && photoIds.Any())
            {
                foreach (var id in photoIds)
                {
                    await _fileRepository.AddAsync(new ProductFile<DAL.Data.Entities.Review>()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = entityId,
                        MinioFileId = id
                    });
                }
            }
        }
    }
}
