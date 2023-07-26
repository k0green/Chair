using Chair.BLL.BusinessLogic.Review;
using Chair.BLL.CQRS.Review;
using Chair.BLL.Dto.Review;
using MediatR;

namespace Chair.BLL.MediatR.Review
{
    public class GetAllReviewsForServiceHandler : IRequestHandler<GetAllReviewsForServiceQuery, List<ReviewDto>>
    {
        private readonly IReviewBusinessLogic _reviewBusinessLogic;

        public GetAllReviewsForServiceHandler(IReviewBusinessLogic reviewBusinessLogic)
        {
            _reviewBusinessLogic = reviewBusinessLogic;
        }

        public async Task<List<ReviewDto>> Handle(GetAllReviewsForServiceQuery request, CancellationToken cancellationToken)
        {
            var result = await _reviewBusinessLogic.GetAllReviewsForService(request.ExecutorServiceId);

            return result;
        }
    }
}
