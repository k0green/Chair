using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.CQRS.ServiceType
{
    public class AddExecutorProfileQuery : IRequest<Guid>
    {
        public AddExecutorProfileDto AddExecutorProfileDto { get; set; }
    }
}
