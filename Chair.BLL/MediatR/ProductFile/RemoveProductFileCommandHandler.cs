using Chair.BLL.BusinessLogic.ProductFile;
using Chair.BLL.CQRS.ProductFile;
using Chair.BLL.Dto.ProductFile;
using MediatR;

namespace Chair.BLL.MediatR.ProductFile;

public class RemoveProductFileCommandHandler<TProductFile>
    : IRequestHandler<RemoveProductFileCommand<TProductFile>, Unit>
    where TProductFile : DAL.Data.Entities.ProductFile, new()
{
    private readonly ProductFileBusinessLogic<TProductFile, FileSaveDto, object> _productFileBusinessLogic;

    public RemoveProductFileCommandHandler(ProductFileBusinessLogic<TProductFile, FileSaveDto, object> productFileBusinessLogic)
    {
        _productFileBusinessLogic = productFileBusinessLogic;
    }

    public async Task<Unit> Handle(RemoveProductFileCommand<TProductFile> request, CancellationToken cancellationToken)
    {
        await _productFileBusinessLogic.RemoveAsync(request.ProductId, request.FileId);

        return Unit.Value;
    }
}