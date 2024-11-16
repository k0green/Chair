using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using Chair.BLL.Dto.Minio;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.YandexCloud
{
    public class YandexCloudBusinessLogic : IYandexCloudBusinessLogic
    {
        private const string BucketName = "chair";
        private readonly AmazonS3Client _s3Client;
        private readonly IBaseWithManyRepository<MinioFile> _fileRepository;

        public YandexCloudBusinessLogic(IBaseWithManyRepository<MinioFile> fileRepository)
        {
            _fileRepository = fileRepository;

            var credentials = new BasicAWSCredentials("key", "key");

            var config = new AmazonS3Config
            {
                ServiceURL = "https://storage.yandexcloud.net",
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(credentials, config);
        }

        public async Task<MinioFileDto> UploadFile(AddMinioFileDto dto)
        {
            var entity = new MinioFile
            {
                Id = Guid.NewGuid(),
                Name = dto.FileName,
                CreateDate = DateTime.UtcNow,
            };

            var fileExtension = Path.GetExtension(dto.FileName);
            var objectKey = $"{entity.Id}{fileExtension}";

            // Upload to Yandex Cloud S3
            var putRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = objectKey,
                InputStream = dto.FileData,
                ContentType = "application/octet-stream"
            };

            await _s3Client.PutObjectAsync(putRequest);

            // Generate pre-signed URL (7 days expiration)
            var presignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = BucketName,
                Key = objectKey,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            var url = _s3Client.GetPreSignedURL(presignedUrlRequest);
            entity.Url = url.Split("?").First();

            // Save file metadata to the database
            await _fileRepository.AddAsync(entity);
            await _fileRepository.SaveChangesAsync();

            return new MinioFileDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Url = entity.Url,
                CreateDate = entity.CreateDate
            };
        }

        public async Task<MinioFileFullDto> DownloadFile(Guid fileId)
        {
            var data = await _fileRepository.GetByIdAsync(fileId);
            var fileExtension = Path.GetExtension(data.Name);
            var objectKey = $"{data.Id}{fileExtension}";

            try
            {
                // Create a request to download the file
                var request = new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = objectKey
                };

                using (var response = await _s3Client.GetObjectAsync(request))
                using (var stream = new MemoryStream())
                {
                    await response.ResponseStream.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    return new MinioFileFullDto
                    {
                        Id = data.Id,
                        Name = data.Name,
                        File = stream.ToArray(),
                        Url = data.Url,
                        CreateDate = data.CreateDate
                    };
                }
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Yandex Cloud S3 Error: {ex.Message}");
            }
        }

        public async Task DeleteFile(Guid fileId)
        {
            var data = await _fileRepository.GetAllByPredicateAsQueryable(x => x.Id == fileId).FirstOrDefaultAsync();
            var fileExtension = Path.GetExtension(data.Name);
            var objectKey = $"{data.Id}{fileExtension}";

            try
            {
                // Create request to delete file
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = BucketName,
                    Key = objectKey
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);
                await _fileRepository.RemoveAsync(data);
                await _fileRepository.SaveChangesAsync();
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Yandex Cloud S3 Error: {ex.Message}");
            }
        }
    }
}
