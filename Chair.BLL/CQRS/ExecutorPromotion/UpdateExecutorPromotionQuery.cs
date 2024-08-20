using Chair.BLL.Dto.ExecutorPromotion;
using MediatR;

namespace Chair.BLL.CQRS.ExecutorPromotion
{
    public class UpdateExecutorPromotionQuery : IRequest<Unit>
    {
        public UpdateExecutorPromotionDto UpdateExecutorPromotionDto { get; set; }
    }
}
