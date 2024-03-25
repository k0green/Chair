using Chair.BLL.BusinessLogic.Minio;
using Chair.BLL.CQRS.Minio;
using Chair.BLL.Dto.Minio;
using MediatR;

namespace Chair.BLL.MediatR.Minio;

public class DownloadMinioFileQueryHandler : IRequestHandler<DownloadMinioFileQuery, MinioFileFullDto>
{
    private readonly IMinioBusinessLogic _minioBusinessLogic;

    public DownloadMinioFileQueryHandler(IMinioBusinessLogic minioBusinessLogic)
    {
        _minioBusinessLogic = minioBusinessLogic;
    }

    public async Task<MinioFileFullDto> Handle(DownloadMinioFileQuery request, CancellationToken cancellationToken)
    {
        var result = await _minioBusinessLogic.DownloadFile(request.Id);

        return result;
    }
}