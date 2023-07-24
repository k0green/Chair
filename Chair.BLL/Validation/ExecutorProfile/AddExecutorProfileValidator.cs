using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class AddExecutorProfileValidator : AbstractValidator<AddExecutorProfileQuery>
    {
        private readonly ApplicationDbContext _context;
        public AddExecutorProfileValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.AddExecutorProfileDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.AddExecutorProfileDto.UserId).MustAsync(async (id, token) =>
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                return user != null;
            }).WithMessage("User with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.AddExecutorProfileDto.UserId).MustAsync(async (id, token) =>
            {
                var user = await _context.ExecutorProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);

                return user == null;
            }).WithMessage("User with Id: {PropertyValue} has a profile");
        }
    }
}
