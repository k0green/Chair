using Chair.BLL.BusinessLogic.ProductFile;
using Chair.BLL.CQRS.ProductFile;
using Chair.BLL.Dto.ProductFile;
using MediatR;

namespace Chair.BLL.MediatR.ProductFile;

public class AddProductFileCommandHandler<TProductFile, TFileSaveDto>
    : IRequestHandler<AddProductFileCommand<TProductFile, TFileSaveDto>, Unit>
    where TProductFile : DAL.Data.Entities.ProductFile, new()
    where TFileSaveDto : FileSaveDto
{
    private readonly ProductFileBusinessLogic<TProductFile, TFileSaveDto, object> _productFileBusinessLogic;

    public AddProductFileCommandHandler(ProductFileBusinessLogic<TProductFile, TFileSaveDto, object> productFileBusinessLogic)
    {
        _productFileBusinessLogic = productFileBusinessLogic;
    }

    public async Task<Unit> Handle(AddProductFileCommand<TProductFile, TFileSaveDto> request, CancellationToken cancellationToken)
    {
        await _productFileBusinessLogic.AddAsync(request.ProductId, request.File);

        return Unit.Value;
    }
}