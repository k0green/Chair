using Chair.BLL.BusinessLogic.ProductFile;
using Chair.BLL.CQRS.ProductFile;
using Chair.BLL.Dto.ProductFile;
using MediatR;

namespace Chair.BLL.MediatR.ProductFile;

public class AddRangeProductFileCommandHandler<TProductFile, TFileSaveDto>
    : IRequestHandler<AddRangeProductFileCommand<TProductFile, TFileSaveDto>, Unit>
    where TProductFile : DAL.Data.Entities.ProductFile, new()
    where TFileSaveDto : FileSaveDto
{
    private readonly ProductFileBusinessLogic<TProductFile, TFileSaveDto, object> _productFileBusinessLogic;

    public AddRangeProductFileCommandHandler(ProductFileBusinessLogic<TProductFile, TFileSaveDto, object> productFileBusinessLogic)
    {
        _productFileBusinessLogic = productFileBusinessLogic;
    }

    public async Task<Unit> Handle(AddRangeProductFileCommand<TProductFile, TFileSaveDto> request, CancellationToken cancellationToken)
    {
        await _productFileBusinessLogic.AddRangeAsync(request.ProductId, request.Files);

        return Unit.Value;
    }
}