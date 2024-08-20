using MediatR;
using Chair.BLL.Dto.ExecutorPromotion;

namespace Chair.BLL.CQRS.ExecutorPromotion
{
    public class GetAllPromotionsByExecutorIdQuery : IRequest<List<ExecutorPromotionDto>>
    {
        public Guid ExecutorId { get; set; }
    }
}
