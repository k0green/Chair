using Chair.BLL.Dto.ProductFile;
using MediatR;

namespace Chair.BLL.CQRS.ProductFile;

public class AddRangeProductFileCommand<TProductFile, TFileSaveDto> : IRequest<Unit>
    where TProductFile : DAL.Data.Entities.ProductFile, new()
    where TFileSaveDto : FileSaveDto
{
    public Guid ProductId { get; set; }
    public List<TFileSaveDto> Files { get; set; }
}