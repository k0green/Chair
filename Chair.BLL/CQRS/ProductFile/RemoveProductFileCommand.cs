using MediatR;

namespace Chair.BLL.CQRS.ProductFile;

public class RemoveProductFileCommand<TProductFile> : IRequest<Unit>
    where TProductFile : DAL.Data.Entities.ProductFile, new()
{
    public Guid ProductId { get; set; }
    public Guid FileId { get; set; }
}