using Chair.BLL.BusinessLogic.ExecutorPromotion;
using Chair.BLL.CQRS.ExecutorPromotion;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorPromotion
{
    public class AddExecutorPromotionHandler : IRequestHandler<AddExecutorPromotionQuery, Guid>
    {
        private readonly IExecutorPromotionBusinessLogic _executorPromotion;

        public AddExecutorPromotionHandler(IExecutorPromotionBusinessLogic executorPromotionBusinessLogic)
        {
            _executorPromotion = executorPromotionBusinessLogic;
        }

        public async Task<Guid> Handle(AddExecutorPromotionQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorPromotion.AddAsync(request.AddExecutorPromotionDto);

            return result;
        }
    }
}
