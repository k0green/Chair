using Chair.BLL.Dto.Base;
using MediatR;

namespace Chair.BLL.CQRS.ExecutorService
{
    public class GetAllServicesNamesQuery : IRequest<List<LookupDto>>
    {
    }
}
