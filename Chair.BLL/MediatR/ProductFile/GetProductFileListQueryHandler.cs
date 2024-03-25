using Chair.BLL.BusinessLogic.ProductFile;
using Chair.BLL.CQRS.ProductFile;
using Chair.BLL.Dto.ProductFile;
using MediatR;

namespace Chair.BLL.MediatR.ProductFile;

public class GetProductFileListQueryHandler<TProductFile, TFileViewDto>
    : IRequestHandler<GetProductFileListQuery<TProductFile, TFileViewDto>, List<TFileViewDto>>
    where TProductFile : DAL.Data.Entities.ProductFile, new()
    where TFileViewDto : class
{
    private readonly ProductFileBusinessLogic<TProductFile, FileSaveDto, TFileViewDto> _productFileBusinessLogic;

    public GetProductFileListQueryHandler(ProductFileBusinessLogic<TProductFile, FileSaveDto, TFileViewDto> productFileBusinessLogic)
    {
        _productFileBusinessLogic = productFileBusinessLogic;
    }

    public async Task<List<TFileViewDto>> Handle(GetProductFileListQuery<TProductFile, TFileViewDto> request, CancellationToken cancellationToken)
    {
        var result = await _productFileBusinessLogic.GetAllFilesByProductIdAsync(request.ProductId);

        return result;
    }
}
