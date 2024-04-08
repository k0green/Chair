using MediatR;

namespace Chair.BLL.CQRS.ProductFile;

public class RemoveRangeProductFileCommand<TProductFile> : IRequest<Unit>
    where TProductFile : DAL.Data.Entities.ProductFile
{
    public Guid ProductId { get; set; }
    public List<Guid> FileIds { get; set; }
}