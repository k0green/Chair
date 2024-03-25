using MediatR;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.CQRS.Minio;

public class UploadRangeMinioFileCommand : IRequest<List<Guid>>
{
    public List<AddMinioFileDto> AddMinioFileDto { get; set; }
}
