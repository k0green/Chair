using Chair.BLL.CQRS.Review;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Review
{
    public class GetAllReviewsForServiceValidator : AbstractValidator<GetAllReviewsForServiceQuery>
    {
        private readonly ApplicationDbContext _context;
        public GetAllReviewsForServiceValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.ExecutorServiceId).MustAsync(async (id, token) =>
            {
                var executor = await _context.ExecutorServices.FirstOrDefaultAsync(x => x.Id == id);

                return executor != null;
            }).WithMessage("Executor service with Id: {PropertyValue} doesn't exist");
        }
    }
}
