using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class RemoveOrderQuery : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
