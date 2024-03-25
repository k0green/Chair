using Chair.BLL.Dto.ProductFile;
using MediatR;

namespace Chair.BLL.CQRS.ProductFile;

public class AddProductFileCommand<TProductFile, TFileSaveDto> : IRequest<Unit>
    where TProductFile : DAL.Data.Entities.ProductFile, new()
    where TFileSaveDto : FileSaveDto
{
    public Guid ProductId { get; set; }
    public TFileSaveDto File { get; set; }
}