using Chair.BLL.BusinessLogic.ExecutorPromotion;
using Chair.BLL.CQRS.ExecutorPromotion;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorPromotion
{
    public class RemoveExecutorPromotionHandler : IRequestHandler<RemoveExecutorPromotionQuery, Unit>
    {
        private readonly IExecutorPromotionBusinessLogic _executorPromotionBusinessLogic;

        public RemoveExecutorPromotionHandler(IExecutorPromotionBusinessLogic executorPromotionBusinessLogic)
        {
            _executorPromotionBusinessLogic = executorPromotionBusinessLogic;
        }

        public async Task<Unit> Handle(RemoveExecutorPromotionQuery request, CancellationToken cancellationToken)
        {
            await _executorPromotionBusinessLogic.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
