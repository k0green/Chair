using Chair.DAL.Data;
using Chair.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Chair.DAL.Repositories.Review
{
    public class ReviewRepository : BaseRepository<Data.Entities.Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task RemoveManyAsync(List<Data.Entities.Review> models)
        {
            _dbSet.RemoveRange(models);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveManyByIdsAsync(List<Guid> ids)
        {
            var entities = _dbSet.Where(x => ids.Contains(x.Id));
            if (entities == null || entities.Count() < 0)
                throw new ArgumentNullException($"object with this id = {string.Join(",", ids)} not found");
            await RemoveManyAsync(await entities.ToListAsync());
            await _dbContext.SaveChangesAsync();
        }
    }
}
