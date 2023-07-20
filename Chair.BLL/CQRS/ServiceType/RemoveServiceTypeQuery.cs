using MediatR;

namespace Chair.BLL.CQRS.ServiceType
{
    public class RemoveServiceTypeQuery : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
