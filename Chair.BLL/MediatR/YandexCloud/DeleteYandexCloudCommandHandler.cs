using MediatR;
using Chair.BLL.BusinessLogic.YandexCloud;
using Chair.BLL.CQRS.YandexCloud;

namespace Chair.BLL.MediatR.YandexCloud;

public class DeleteYandexCloudCommandHandler : IRequestHandler<DeleteYandexCloudCommand, Unit>
{
    private readonly IYandexCloudBusinessLogic _yandexCloudBusinessLogic;

    public DeleteYandexCloudCommandHandler(IYandexCloudBusinessLogic yandexCloudBusinessLogic)
    {
        _yandexCloudBusinessLogic = yandexCloudBusinessLogic;
    }

    public async Task<Unit> Handle(DeleteYandexCloudCommand request, CancellationToken cancellationToken)
    {
        await _yandexCloudBusinessLogic.DeleteFile(request.Id);

        return Unit.Value;
    }
}
