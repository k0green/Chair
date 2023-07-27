using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class ApproveOrderQuery : IRequest<Unit>
    {
        public Guid OrderId { get; set; }
        public bool IsExecutor { get; set; }
    }
}
