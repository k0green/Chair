using MediatR;

namespace Chair.BLL.CQRS.ServiceType
{
    public class RemoveExecutorServiceQuery : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
