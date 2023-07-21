using MediatR;
using Chair.BLL.Dto.ExecutorService;

namespace Chair.BLL.CQRS.ExecutorService
{
    public class GetAllServicesByExecutorIdQuery : IRequest<List<ExecutorServiceDto>>
    {
        public string ExecutorId { get; set; }
    }
}
