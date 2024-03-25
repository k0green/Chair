using Chair.DAL.Repositories.Base;

namespace Chair.DAL.Repositories.Minio;

public interface IMinioFileRepository : IBaseWithManyRepository<Data.Entities.MinioFile>
{
}
