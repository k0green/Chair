using Chair.BLL.BusinessLogic.YandexCloud;
using Chair.BLL.CQRS.YandexCloud;
using Chair.BLL.Dto.Minio;
using MediatR;

namespace Chair.BLL.MediatR.YandexCloud;

public class DownloadYandexCloudQueryHandler : IRequestHandler<DownloadYandexCloudQuery, MinioFileFullDto>
{
    private readonly IYandexCloudBusinessLogic _yandexCloudBusinessLogic;

    public DownloadYandexCloudQueryHandler(IYandexCloudBusinessLogic yandexCloudBusinessLogic)
    {
        _yandexCloudBusinessLogic = yandexCloudBusinessLogic;
    }

    public async Task<MinioFileFullDto> Handle(DownloadYandexCloudQuery request, CancellationToken cancellationToken)
    {
        var result = await _yandexCloudBusinessLogic.DownloadFile(request.Id);

        return result;
    }
}