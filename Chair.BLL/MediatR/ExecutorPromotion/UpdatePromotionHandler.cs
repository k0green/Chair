using Chair.BLL.BusinessLogic.ExecutorPromotion;
using Chair.BLL.CQRS.ExecutorPromotion;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorPromotion
{
    public class UpdateExecutorPromotionHandler : IRequestHandler<UpdateExecutorPromotionQuery, Unit>
    {
        private readonly IExecutorPromotionBusinessLogic _executorPromotionBusinessLogic;

        public UpdateExecutorPromotionHandler(IExecutorPromotionBusinessLogic executorPromotionBusinessLogic)
        {
            _executorPromotionBusinessLogic = executorPromotionBusinessLogic;
        }

        public async Task<Unit> Handle(UpdateExecutorPromotionQuery request, CancellationToken cancellationToken)
        {
            await _executorPromotionBusinessLogic.UpdateAsync(request.UpdateExecutorPromotionDto);

            return Unit.Value;
        }
    }
}
