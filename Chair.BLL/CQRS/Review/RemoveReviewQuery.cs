using MediatR;

namespace Chair.BLL.CQRS.Review
{
    public class RemoveReviewQuery : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
