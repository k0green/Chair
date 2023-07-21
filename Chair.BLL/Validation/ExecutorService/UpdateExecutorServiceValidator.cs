using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using Microsoft.EntityFrameworkCore.Query;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class UpdateExecutorServiceValidator : AbstractValidator<UpdateExecutorServiceQuery>
    {
        private readonly ApplicationDbContext _context;

        public UpdateExecutorServiceValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.UpdateExecutorServiceDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.UpdateExecutorServiceDto).MustAsync(async (dto, token) =>
            {
                var existingServiceType = await _context.ServiceTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == dto.ServiceTypeId);

                return existingServiceType != null;
            }).WithMessage("Service with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.UpdateExecutorServiceDto.ExecutorId).MustAsync(async (id, token) =>
            {
                var executor = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                return executor != null;
            }).WithMessage("User with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.UpdateExecutorServiceDto.Id).MustAsync(async (id, token) =>
            {
                var serviceType = await _context.ExecutorServices
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                return serviceType != null;
            }).WithMessage("Executor service with Id: {PropertyValue} doesn't exist");
        }
    }
}
