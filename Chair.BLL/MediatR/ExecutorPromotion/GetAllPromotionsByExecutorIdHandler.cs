using Chair.BLL.BusinessLogic.ExecutorPromotion;
using Chair.BLL.CQRS.ExecutorPromotion;
using Chair.BLL.Dto.ExecutorPromotion;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorPromotion
{
    public class GetAllPromotionsByExecutorIdHandler : IRequestHandler<GetAllPromotionsByExecutorIdQuery, List<ExecutorPromotionDto>>
    {
        private readonly IExecutorPromotionBusinessLogic _executorPromotionBusinessLogic;

        public GetAllPromotionsByExecutorIdHandler(IExecutorPromotionBusinessLogic executorPromotionBusinessLogic)
        {
            _executorPromotionBusinessLogic = executorPromotionBusinessLogic;
        }

        public async Task<List<ExecutorPromotionDto>> Handle(GetAllPromotionsByExecutorIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorPromotionBusinessLogic.GetAllPromotionsByExecutorId(request.ExecutorId);

            return result;
        }
    }
}
