using Chair.BLL.Dto.Minio;

namespace Chair.BLL.BusinessLogic.Minio
{
    public interface IMinioBusinessLogic
    {
        Task<MinioFileDto> UploadFile(AddMinioFileDto dto);
        Task<List<MinioFileDto>> UploadRangeFile(List<AddMinioFileDto> dtos);
        Task DeleteFile(Guid fileId);
        Task<MinioFileFullDto> DownloadFile(Guid fileId);
    }
}