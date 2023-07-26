using Chair.BLL.BusinessLogic.Review;
using Chair.BLL.CQRS.Review;
using MediatR;

namespace Chair.BLL.MediatR.Review
{
    public class UpdateReviewHandler : IRequestHandler<UpdateReviewQuery, Unit>
    {
        private readonly IReviewBusinessLogic _reviewBusinessLogic;

        public UpdateReviewHandler(IReviewBusinessLogic reviewBusinessLogic)
        {
            _reviewBusinessLogic = reviewBusinessLogic;
        }

        public async Task<Unit> Handle(UpdateReviewQuery request, CancellationToken cancellationToken)
        {
            await _reviewBusinessLogic.UpdateAsync(request.UpdateReviewDto);

            return Unit.Value;
        }
    }
}
