using Chair.BLL.BusinessLogic.ProductFile;
using Chair.BLL.CQRS.ProductFile;
using Chair.BLL.Dto.ProductFile;
using MediatR;

namespace Chair.BLL.MediatR.ProductFile;

public class RemoveRangeProductFileCommandHandler<TProductFile>
    : IRequestHandler<RemoveRangeProductFileCommand<TProductFile>, Unit>
    where TProductFile : DAL.Data.Entities.ProductFile, new()
{
    private readonly ProductFileBusinessLogic<TProductFile, FileSaveDto, object> _productFileBusinessLogic;

    public RemoveRangeProductFileCommandHandler(ProductFileBusinessLogic<TProductFile, FileSaveDto, object> productFileBusinessLogic)
    {
        _productFileBusinessLogic = productFileBusinessLogic;
    }

    public async Task<Unit> Handle(RemoveRangeProductFileCommand<TProductFile> request, CancellationToken cancellationToken)
    {
        await _productFileBusinessLogic.RemoveRangeAsync(request.ProductId, request.FileIds);

        return Unit.Value;
    }
}