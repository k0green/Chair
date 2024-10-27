using Chair.BLL.Dto.Minio;

namespace Chair.BLL.BusinessLogic.YandexCloud
{
    public interface IYandexCloudBusinessLogic
    {
        Task<MinioFileDto> UploadFile(AddMinioFileDto dto);
        Task DeleteFile(Guid fileId);
        Task<MinioFileFullDto> DownloadFile(Guid fileId);
    }
}