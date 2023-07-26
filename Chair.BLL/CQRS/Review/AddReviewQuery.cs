using Chair.BLL.Dto.Review;
using MediatR;

namespace Chair.BLL.CQRS.Review
{
    public class AddReviewQuery : IRequest<Guid>
    {
        public AddReviewDto AddReviewDto { get; set; }
    }
}
