using MediatR;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.CQRS.Minio;

public class DownloadMinioFileQuery : IRequest<MinioFileFullDto>
{
	public Guid Id { get; set; }
}
