using Chair.BLL.Dto.Review;
using MediatR;

namespace Chair.BLL.CQRS.Review
{
    public class UpdateReviewQuery : IRequest<Unit>
    {
        public UpdateReviewDto UpdateReviewDto { get; set; }
    }
}
