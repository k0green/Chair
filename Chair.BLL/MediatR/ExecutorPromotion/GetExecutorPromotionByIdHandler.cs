using Chair.BLL.BusinessLogic.ExecutorPromotion;
using Chair.BLL.CQRS.ExecutorPromotion;
using Chair.BLL.Dto.ExecutorPromotion;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorPromotion
{
    public class GetExecutorPromotionByIdHandler : IRequestHandler<GetExecutorPromotionByIdQuery, ExecutorPromotionDto>
    {
        private readonly IExecutorPromotionBusinessLogic _executorPromotionBusinessLogic;

        public GetExecutorPromotionByIdHandler(IExecutorPromotionBusinessLogic executorPromotionBusinessLogic)
        {
            _executorPromotionBusinessLogic = executorPromotionBusinessLogic;
        }

        public async Task<ExecutorPromotionDto> Handle(GetExecutorPromotionByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorPromotionBusinessLogic.GetByIdPromotion(request.Id);

            return result;
        }
    }
}
