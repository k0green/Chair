using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class UpdateExecutorProfileValidator : AbstractValidator<UpdateExecutorProfileQuery>
    {
        private readonly ApplicationDbContext _context;

        public UpdateExecutorProfileValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.UpdateExecutorProfileDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.UpdateExecutorProfileDto).MustAsync(async (dto, token) =>
            {
                var executorProfile = await _context.ExecutorProfiles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                return executorProfile != null;
            }).WithMessage("Profile with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.UpdateExecutorProfileDto.UserId).MustAsync(async (id, token) =>
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                return user != null;
            }).WithMessage("User with Id: {PropertyValue} doesn't exist");
        }
    }
}
