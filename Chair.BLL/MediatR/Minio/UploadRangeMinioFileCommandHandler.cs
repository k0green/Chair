using MediatR;
using Chair.BLL.BusinessLogic.Minio;
using Chair.BLL.CQRS.Minio;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.MediatR.Minio;

public class UploadRangeMinioFileCommandHandler : IRequestHandler<UploadRangeMinioFileCommand, List<MinioFileDto>>
{
    private readonly IMinioBusinessLogic _minioBusinessLogic;

    public UploadRangeMinioFileCommandHandler(IMinioBusinessLogic minioBusinessLogic)
    {
        _minioBusinessLogic = minioBusinessLogic;
    }

    public async Task<List<MinioFileDto>> Handle(UploadRangeMinioFileCommand request, CancellationToken cancellationToken)
    {
        var ids = await _minioBusinessLogic.UploadRangeFile(request.AddMinioFileDto);

        return ids;
    }
}
