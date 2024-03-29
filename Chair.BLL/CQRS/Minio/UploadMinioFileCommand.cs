using MediatR;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.CQRS.Minio;

public class UploadMinioFileCommand : IRequest<MinioFileDto>
{
    public AddMinioFileDto AddMinioFileDto { get; set; }
}
