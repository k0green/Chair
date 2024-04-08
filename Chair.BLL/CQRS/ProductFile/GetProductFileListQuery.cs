using MediatR;

namespace Chair.BLL.CQRS.ProductFile;

public class GetProductFileListQuery<TProductFile, TFileViewDto> : IRequest<List<TFileViewDto>>
    where TProductFile : DAL.Data.Entities.ProductFile
    where TFileViewDto : class
{
    public Guid ProductId { get; set; }
}