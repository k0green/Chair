using MediatR;

namespace Chair.BLL.CQRS.ServiceType
{
    public class RemoveExecutorProfileQuery : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
