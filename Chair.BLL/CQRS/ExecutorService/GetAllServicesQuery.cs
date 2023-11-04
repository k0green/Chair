using MediatR;
using Chair.BLL.Dto.ExecutorService;

namespace Chair.BLL.CQRS.ExecutorService
{
    public class GetAllServicesQuery : IRequest<List<GroupExecutorServiceDto>>
    {
    }
}
