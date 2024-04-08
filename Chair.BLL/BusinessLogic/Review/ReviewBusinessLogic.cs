using AutoMapper;
using Chair.BLL.Dto.Review;
using Chair.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.Review
{
    public class ReviewBusinessLogic : IReviewBusinessLogic
    {
        private readonly IBaseWithManyRepository<DAL.Data.Entities.Review> _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewBusinessLogic(IBaseWithManyRepository<DAL.Data.Entities.Review> reviewRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<List<ReviewDto>> GetAllReviewsForService(Guid executorServiceId)
        {
            var allReviews = await _reviewRepository
                .GetAllByPredicateAsQueryable(x => x.ExecutorServiceId == executorServiceId)
                .Include(x=>x.ExecutorService.Executor.User)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var parentReviews = allReviews.Where(x => x.ParentId == null).ToList();
            var parentReviewDtos = _mapper.Map<List<ReviewDto>>(parentReviews);

            foreach (var pR in parentReviewDtos)
            {
                pR.Child = _mapper.Map<List<ReviewDto>>(allReviews.Where(x => x.ParentId == pR.Id));
            }

            return parentReviewDtos;
        }

        public async Task<Guid> AddAsync(AddReviewDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.Review>(dto);
            entity.Id = Guid.NewGuid();
            entity.CreateDate = DateTime.Now;

            await _reviewRepository.AddAsync(entity);
            await _reviewRepository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(UpdateReviewDto dto)
        {
            var entity = await _reviewRepository.GetByIdAsync(dto.Id);
            _mapper.Map(dto, entity);
            await _reviewRepository.UpdateAsync(entity);
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
    }
}
