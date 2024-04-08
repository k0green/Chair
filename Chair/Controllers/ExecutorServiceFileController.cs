using Chair.BLL.Dto.ProductFile;
using Chair.DAL.Data.Entities;
using MediatR;

namespace Chair.Controllers;

public class ExecutorServiceFileController : ProductFileController<ProductFile>
{
    public ExecutorServiceFileController(IMediator mediator) : base(mediator)
    {
    }
}