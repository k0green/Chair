using Chair.BLL.Dto.ExecutorPromotion;
using MediatR;

namespace Chair.BLL.CQRS.ExecutorPromotion
{
    public class AddExecutorPromotionQuery : IRequest<Guid>
    {
        public AddExecutorPromotionDto AddExecutorPromotionDto { get; set; }
    }
}
