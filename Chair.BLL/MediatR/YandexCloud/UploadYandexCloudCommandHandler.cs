using MediatR;
using Chair.BLL.BusinessLogic.YandexCloud;
using Chair.BLL.CQRS.YandexCloud;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.MediatR.YandexCloud;

public class UploadYandexCloudCommandHandler : IRequestHandler<UploadYandexCloudCommand, MinioFileDto>
{
    private readonly IYandexCloudBusinessLogic _yandexCloudBusinessLogic;

    public UploadYandexCloudCommandHandler(IYandexCloudBusinessLogic yandexCloudBusinessLogic)
    {
        _yandexCloudBusinessLogic = yandexCloudBusinessLogic;
    }

    public async Task<MinioFileDto> Handle(UploadYandexCloudCommand request, CancellationToken cancellationToken)
    {
        var dto = await _yandexCloudBusinessLogic.UploadFile(request.AddMinioFileDto);

        return dto;
    }
}
