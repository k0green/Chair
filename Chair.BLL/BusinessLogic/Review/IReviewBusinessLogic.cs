using Chair.BLL.Dto.Review;

namespace Chair.BLL.BusinessLogic.Review
{
    public interface IReviewBusinessLogic
    {
        Task<List<ReviewDto>> GetAllReviewsForService(Guid executorServiceId);
        Task<Guid> AddAsync(AddReviewDto dto);
        Task UpdateAsync(UpdateReviewDto dto);
        Task RemoveAsync(Guid id);
    }
}
