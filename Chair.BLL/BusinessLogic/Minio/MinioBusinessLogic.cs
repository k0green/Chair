using Chair.BLL.Dto.Minio;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.Exceptions;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Minio;
using Minio.DataModel.Args;

namespace Chair.BLL.BusinessLogic.Minio
{
    public class MinioBusinessLogic : IMinioBusinessLogic
    {
        private const string BucketName = "erp-files";
        private const string ContentType = "application/octet-stream";

        private readonly MinioClient _minio;
        private readonly IMinioFileRepository _minioFileRepository;

        public MinioBusinessLogic(IMinioFileRepository minioFileRepository, MinioClient minioClient)
        {
            _minioFileRepository = minioFileRepository;
            _minio = minioClient;
        }

        public async Task<MinioFileDto> UploadFile(AddMinioFileDto dto)
        {
            await CheckBucket();
            return await Upload(dto);
        }

        public async Task<List<MinioFileDto>> UploadRangeFile(List<AddMinioFileDto> dtos)
        {
            await CheckBucket();
            var ids = new List<MinioFileDto>();
            foreach (var dto in dtos)
            {
                ids.Add(await Upload(dto));
            }
            return ids;
        }

        public async Task DeleteFile(Guid fileId)
        {
            var data = await _minioFileRepository.GetAllByPredicateAsQueryable(x => x.Id == fileId).FirstOrDefaultAsync();
            var fileExtension = Path.GetExtension(data.Name);

            var objectNameWithExtension = $"{data.Id}{fileExtension}";
            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(objectNameWithExtension);
                var stat = await _minio.StatObjectAsync(statObjectArgs).ConfigureAwait(false);

                if (stat != null)
                {
                    var removeObjectArgs = new RemoveObjectArgs()
                        .WithBucket(BucketName)
                        .WithObject(objectNameWithExtension);
                    await _minio.RemoveObjectAsync(removeObjectArgs).ConfigureAwait(false);
                    await _minioFileRepository.RemoveAsync(data);
                    await _minioFileRepository.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"File {data.Name} does not exist in {BucketName}");
                }
            }
            catch (MinioException e)
            {
                throw new Exception($"MinIO Error: {e.Message}");
            }
        }

        public async Task<MinioFileFullDto> DownloadFile(Guid fileId)
        {
            var data = await _minioFileRepository.GetByIdAsync(fileId);
            var fileExtension = Path.GetExtension(data.Name);

            var objectNameWithExtension = $"{data.Id}{fileExtension}";
            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(objectNameWithExtension);
                var stat = await _minio.StatObjectAsync(statObjectArgs).ConfigureAwait(false);

                if (stat != null)
                {
                    var stream = new MemoryStream();
                    var getObjectArgs = new GetObjectArgs()
                        .WithBucket(BucketName)
                        .WithObject(objectNameWithExtension)
                        .WithCallbackStream((responseStream) =>
                        {
                            responseStream.CopyTo(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                        });


                    await _minio.GetObjectAsync(getObjectArgs).ConfigureAwait(false);
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
                        byte[] byteArray = memoryStream.ToArray();
                    }
                    stream.Seek(0, SeekOrigin.Begin);

                    byte[] fileBytes = stream.ToArray();

                    return new MinioFileFullDto
                    {
                        Id = data.Id,
                        Name = data.Name,
                        CreateDate = data.CreateDate,
                        File = fileBytes,
                        Url = data.Url,
                    };
                }
                else
                {
                    throw new Exception($"File {$"{objectNameWithExtension}"} does not exist in {BucketName}");
                }
            }
            catch (MinioException e)
            {
                throw new Exception($"MinIO Error: {e.Message}");
            }
        }

        private async Task CheckBucket()
        {
            try
            {
                var mbArgs = new MakeBucketArgs().WithBucket(BucketName).WithObjectLock();
                var beArgs = new BucketExistsArgs().WithBucket(BucketName);
                var found = await _minio.BucketExistsAsync(beArgs);
                if (!found)
                {
                    await _minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
            }
            catch (MinioException e)
            {
                throw new Exception($"MinIO bucket Error: {e.Message}");
            }            
        }

        private async Task<MinioFileDto> Upload(AddMinioFileDto dto)
        {
            try
            {
                var mbArgs = new MakeBucketArgs().WithBucket(BucketName).WithObjectLock();
                var beArgs = new BucketExistsArgs().WithBucket(BucketName);
                var found = await _minio.BucketExistsAsync(beArgs);
                if (!found)
                {
                    await _minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }

                var entity = new MinioFile
                {
                    Id = Guid.NewGuid(),
                    Name = dto.FileName,
                    CreateDate = DateTime.UtcNow,
                };

                long objectSize = dto.FileData.Length;

                var fileExtension = Path.GetExtension(dto.FileName);

                var objectNameWithExtension = $"{entity.Id}{fileExtension}";

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(objectNameWithExtension)
                    .WithObjectSize(objectSize)
                    .WithStreamData(dto.FileData)
                    .WithContentType(ContentType);
                await _minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                
                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(objectNameWithExtension)
                    .WithExpiry(604800)
                    .WithRequestDate(DateTime.UtcNow);
                
                var presignedUrl = await _minio.PresignedGetObjectAsync(presignedGetObjectArgs);

                entity.Url = presignedUrl;
                
                await _minioFileRepository.AddAsync(entity);
                await _minioFileRepository.SaveChangesAsync();

                return new MinioFileDto()
                {
                    CreateDate = entity.CreateDate,
                    Id = entity.Id,
                    Name = entity.Name,
                    Url = entity.Url,
                };
            }
            catch (MinioException e)
            {
                throw new Exception($"MinIO Error: {e.Message}");
            }
        }
        
        public async Task UpdateFileLinksAsync()
        {
            try
            {
                var oldFiles = _minioFileRepository.GetAllAsync().Where(f => f.CreateDate <= DateTime.UtcNow.AddDays(-6));

                foreach (var file in oldFiles)
                {
                    var fileExtension = Path.GetExtension(file.Name);
                    
                    var objectNameWithExtension = $"{file.Id}{fileExtension}";

                    var presignedGetObjectArgs = new PresignedGetObjectArgs()
                        .WithBucket(BucketName)
                        .WithObject(objectNameWithExtension)
                        .WithExpiry(604800)
                        .WithRequestDate(DateTime.UtcNow);

                    var presignedUrl = await _minio.PresignedGetObjectAsync(presignedGetObjectArgs);

                    file.Url = presignedUrl;

                    await _minioFileRepository.UpdateAsync(file);
                }

                await _minioFileRepository.SaveChangesAsync();
            }
            catch (MinioException e)
            {
                throw new Exception($"MinIO Error: {e.Message}");
            }
        }


    }
}
