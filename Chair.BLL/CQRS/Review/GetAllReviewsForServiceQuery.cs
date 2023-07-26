using Chair.BLL.Dto.Review;
using MediatR;

namespace Chair.BLL.CQRS.Review
{
    public class GetAllReviewsForServiceQuery : IRequest<List<ReviewDto>>
    {
        public Guid ExecutorServiceId { get; set; }
    }
}
