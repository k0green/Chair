using MediatR;
using Chair.BLL.BusinessLogic.Minio;
using Chair.BLL.CQRS.Minio;

namespace Chair.BLL.MediatR.Minio;

public class DeleteMinioFileCommandHandler : IRequestHandler<DeleteMinioFileCommand, Unit>
{
    private readonly IMinioBusinessLogic _minioBusinessLogic;

    public DeleteMinioFileCommandHandler(IMinioBusinessLogic minioBusinessLogic)
    {
        _minioBusinessLogic = minioBusinessLogic;
    }

    public async Task<Unit> Handle(DeleteMinioFileCommand request, CancellationToken cancellationToken)
    {
        await _minioBusinessLogic.DeleteFile(request.Id);

        return Unit.Value;
    }
}
