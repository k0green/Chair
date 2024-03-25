using MediatR;
using Chair.BLL.BusinessLogic.Minio;
using Chair.BLL.CQRS.Minio;

namespace Chair.BLL.MediatR.Minio;

public class UploadRangeMinioFileCommandHandler : IRequestHandler<UploadRangeMinioFileCommand, List<Guid>>
{
    private readonly IMinioBusinessLogic _minioBusinessLogic;

    public UploadRangeMinioFileCommandHandler(IMinioBusinessLogic minioBusinessLogic)
    {
        _minioBusinessLogic = minioBusinessLogic;
    }

    public async Task<List<Guid>> Handle(UploadRangeMinioFileCommand request, CancellationToken cancellationToken)
    {
        var ids = await _minioBusinessLogic.UploadRangeFile(request.AddMinioFileDto);

        return ids;
    }
}
