using Chair.BLL.Dto.Minio;

namespace Chair.BLL.BusinessLogic.Minio
{
    public interface IMinioBusinessLogic
    {
        Task<Guid> UploadFile(AddMinioFileDto dto);
        Task<List<Guid>> UploadRangeFile(List<AddMinioFileDto> dtos);
        Task DeleteFile(Guid fileId);
        Task<MinioFileFullDto> DownloadFile(Guid fileId);
    }
}