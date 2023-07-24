using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class AddExecutorServiceValidator : AbstractValidator<AddExecutorServiceQuery>
    {
        private readonly ApplicationDbContext _context;
        public AddExecutorServiceValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.AddExecutorServiceDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.AddExecutorServiceDto.ServiceTypeId).MustAsync(async (id, token) =>
            {
                var serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Id == id);

                return serviceType != null;
            }).WithMessage("service with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.AddExecutorServiceDto.ExecutorId).MustAsync(async (id, token) =>
            {
                var userProfile = await _context.ExecutorProfiles.FirstOrDefaultAsync(x => x.Id == id);

                return userProfile != null;
            }).WithMessage("user profile with id: {PropertyValue} doesn't exists");
        }
    }
}
