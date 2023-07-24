using Chair.DAL.Data;
using Chair.DAL.Repositories.Base;

namespace Chair.DAL.Repositories.ExecutorService
{
    public class ImageRepository : BaseWithManyRepository<Data.Entities.Image>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
