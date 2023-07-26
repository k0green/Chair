using Chair.BLL.CQRS.Review;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Review
{
    public class AddReviewValidator : AbstractValidator<AddReviewQuery>
    {
        private readonly ApplicationDbContext _context;
        public AddReviewValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.AddReviewDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.AddReviewDto.ExecutorServiceId).MustAsync(async (id, token) =>
            {
                var review = await _context.ExecutorServices.FirstOrDefaultAsync(x => x.Id == id);

                return review != null;
            }).WithMessage("service with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.AddReviewDto.UserId).MustAsync(async (id, token) =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                return user != null;
            }).WithMessage("user  with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.AddReviewDto.ParentId).MustAsync(async (id, token) =>
            {
                if(id == null) 
                    return true;
                var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

                return review != null;
            }).WithMessage("review  with id: {PropertyValue} doesn't exists");
        }
    }
}
