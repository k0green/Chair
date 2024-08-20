using MediatR;

namespace Chair.BLL.CQRS.ExecutorPromotion
{
    public class RemoveExecutorPromotionQuery : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
