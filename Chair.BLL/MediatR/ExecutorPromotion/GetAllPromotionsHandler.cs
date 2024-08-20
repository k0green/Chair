using Chair.BLL.BusinessLogic.ExecutorPromotion;
using Chair.BLL.CQRS.ExecutorPromotion;
using Chair.BLL.Dto.ExecutorPromotion;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorPromotion
{
    public class GetAllPromotionsHandler : IRequestHandler<GetAllPromotionsQuery, (List<ExecutorPromotionDto>, int)>
    {
        private readonly IExecutorPromotionBusinessLogic _executorPromotionBusinessLogic;

        public GetAllPromotionsHandler(IExecutorPromotionBusinessLogic executorPromotionBusinessLogic)
        {
            _executorPromotionBusinessLogic = executorPromotionBusinessLogic;
        }

        public async Task<(List<ExecutorPromotionDto>, int)> Handle(GetAllPromotionsQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorPromotionBusinessLogic.GetAllPromotions(request.Filter);

            return result;
        }
    }
}
