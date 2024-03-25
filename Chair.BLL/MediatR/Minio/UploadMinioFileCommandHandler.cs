using MediatR;
using Chair.BLL.BusinessLogic.Minio;
using Chair.BLL.CQRS.Minio;

namespace Chair.BLL.MediatR.Minio;

public class UploadMinioFileCommandHandler : IRequestHandler<UploadMinioFileCommand, Guid>
{
    private readonly IMinioBusinessLogic _minioBusinessLogic;

    public UploadMinioFileCommandHandler(IMinioBusinessLogic minioBusinessLogic)
    {
        _minioBusinessLogic = minioBusinessLogic;
    }

    public async Task<Guid> Handle(UploadMinioFileCommand request, CancellationToken cancellationToken)
    {
        var id = await _minioBusinessLogic.UploadFile(request.AddMinioFileDto);

        return id;
    }
}
