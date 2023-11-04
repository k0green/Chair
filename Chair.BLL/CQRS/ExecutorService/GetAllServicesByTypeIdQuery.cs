using MediatR;
using Chair.BLL.Dto.ExecutorService;

namespace Chair.BLL.CQRS.ExecutorService
{
    public class GetAllServicesByTypeIdQuery : IRequest<List<GroupExecutorServiceDto>>
    {
        public Guid TypeId { get; set; }
    }
}
