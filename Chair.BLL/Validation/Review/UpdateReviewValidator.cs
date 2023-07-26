using Chair.BLL.CQRS.Review;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Review
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewQuery>
    {
        private readonly ApplicationDbContext _context;

        public UpdateReviewValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.UpdateReviewDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.UpdateReviewDto.Id).MustAsync(async (id, token) =>
            {
                var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

                return review != null;
            }).WithMessage("Review with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.UpdateReviewDto.ExecutorServiceId).MustAsync(async (id, token) =>
            {
                var review = await _context.ExecutorServices.FirstOrDefaultAsync(x => x.Id == id);

                return review != null;
            }).WithMessage("service with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.UpdateReviewDto.UserId).MustAsync(async (id, token) =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                return user != null;
            }).WithMessage("user  with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.UpdateReviewDto.ParentId).MustAsync(async (id, token) =>
            {
                if (id == null)
                    return true;
                var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

                return review != null;
            }).WithMessage("review  with id: {PropertyValue} doesn't exists");
        }
    }
}
