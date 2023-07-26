using Chair.BLL.CQRS.Review;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Review
{
    public class RemoveReviewValidator : AbstractValidator<RemoveReviewQuery>
    {
        private readonly ApplicationDbContext _context;
        public RemoveReviewValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).MustAsync(async (id, token) =>
            {
                var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

                return review != null;
            }).WithMessage("Review with Id: {PropertyValue} doesn't exist");
        }
    }
}
