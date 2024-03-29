using MediatR;
using Chair.BLL.BusinessLogic.Minio;
using Chair.BLL.CQRS.Minio;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.MediatR.Minio;

public class UploadMinioFileCommandHandler : IRequestHandler<UploadMinioFileCommand, MinioFileDto>
{
    private readonly IMinioBusinessLogic _minioBusinessLogic;

    public UploadMinioFileCommandHandler(IMinioBusinessLogic minioBusinessLogic)
    {
        _minioBusinessLogic = minioBusinessLogic;
    }

    public async Task<MinioFileDto> Handle(UploadMinioFileCommand request, CancellationToken cancellationToken)
    {
        var dto = await _minioBusinessLogic.UploadFile(request.AddMinioFileDto);

        return dto;
    }
}
