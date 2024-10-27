using MediatR;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.CQRS.YandexCloud;

public class DownloadYandexCloudQuery : IRequest<MinioFileFullDto>
{
	public Guid Id { get; set; }
}
