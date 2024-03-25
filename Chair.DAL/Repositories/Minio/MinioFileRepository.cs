using Chair.DAL.Data;
using Chair.DAL.Repositories.Base;

namespace Chair.DAL.Repositories.Minio;

public class MinioFileRepository : BaseWithManyRepository<Data.Entities.MinioFile>, IMinioFileRepository
{
	public MinioFileRepository(ApplicationDbContext context) : base(context)
	{
	}
}
