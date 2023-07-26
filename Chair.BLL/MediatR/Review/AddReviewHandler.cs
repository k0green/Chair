using Chair.BLL.BusinessLogic.Review;
using Chair.BLL.CQRS.Review;
using MediatR;

namespace Chair.BLL.MediatR.Review
{
    public class AddReviewHandler : IRequestHandler<AddReviewQuery, Guid>
    {
        private readonly IReviewBusinessLogic _reviewBusinessLogic;

        public AddReviewHandler(IReviewBusinessLogic reviewBusinessLogic)
        {
            _reviewBusinessLogic = reviewBusinessLogic;
        }

        public async Task<Guid> Handle(AddReviewQuery request, CancellationToken cancellationToken)
        {
            var result = await _reviewBusinessLogic.AddAsync(request.AddReviewDto);

            return result;
        }
    }
}
