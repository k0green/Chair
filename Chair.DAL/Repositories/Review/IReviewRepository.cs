using Chair.DAL.Repositories.Base;

namespace Chair.DAL.Repositories.Review
{
    public interface IReviewRepository : IBaseRepository<Data.Entities.Review>
    {
        public Task RemoveManyAsync(List<Data.Entities.Review> models);
        public Task RemoveManyByIdsAsync(List<Guid> ids);
    }
}
