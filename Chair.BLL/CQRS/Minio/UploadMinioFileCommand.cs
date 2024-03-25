using MediatR;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.CQRS.Minio;

public class UploadMinioFileCommand : IRequest<Guid>
{
    public AddMinioFileDto AddMinioFileDto { get; set; }
}
