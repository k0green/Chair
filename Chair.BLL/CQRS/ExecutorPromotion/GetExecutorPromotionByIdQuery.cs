using MediatR;
using Chair.BLL.Dto.ExecutorPromotion;

namespace Chair.BLL.CQRS.ExecutorPromotion
{
    public class GetExecutorPromotionByIdQuery : IRequest<ExecutorPromotionDto>
    {
        public Guid Id { get; set; }
    }
}
