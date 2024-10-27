using MediatR;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.CQRS.YandexCloud;

public class UploadYandexCloudCommand : IRequest<MinioFileDto>
{
    public AddMinioFileDto AddMinioFileDto { get; set; }
}
