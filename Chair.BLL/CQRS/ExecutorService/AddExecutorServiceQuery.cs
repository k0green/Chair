using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.CQRS.ServiceType
{
    public class AddExecutorServiceQuery : IRequest<Guid>
    {
        public AddExecutorServiceDto AddExecutorServiceDto { get; set; }
    }
}
