using Chair.BLL.BusinessLogic.Review;
using Chair.BLL.CQRS.Review;
using MediatR;

namespace Chair.BLL.MediatR.Review
{
    public class RemoveReviewHandler : IRequestHandler<RemoveReviewQuery, Unit>
    {
        private readonly IReviewBusinessLogic _reviewBusinessLogic;

        public RemoveReviewHandler(IReviewBusinessLogic reviewBusinessLogic)
        {
            _reviewBusinessLogic = reviewBusinessLogic;
        }

        public async Task<Unit> Handle(RemoveReviewQuery request, CancellationToken cancellationToken)
        {
            await _reviewBusinessLogic.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
