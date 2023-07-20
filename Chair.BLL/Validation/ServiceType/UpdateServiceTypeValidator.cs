using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using Microsoft.EntityFrameworkCore.Query;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class UpdateServiceTypeValidator : AbstractValidator<UpdateServiceTypeQuery>
    {
        private readonly ApplicationDbContext _context;

        public UpdateServiceTypeValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.ServiceTypeDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.ServiceTypeDto).MustAsync(async (dto, token) =>
            {
                var existingServiceType = await _context.ServiceTypes
                    .AsNoTracking() // Detach the previous tracking of the entity
                    .FirstOrDefaultAsync(x => x.Name == dto.Name);

                return existingServiceType == null || existingServiceType.Id == dto.Id;
            }).WithMessage("Service with name: {PropertyValue} exists");

            RuleFor(x => x.ServiceTypeDto.Id).MustAsync(async (id, token) =>
            {
                var serviceType = await _context.ServiceTypes
                    .AsNoTracking() // Detach the previous tracking of the entity
                    .FirstOrDefaultAsync(x => x.Id == id);

                return serviceType != null;
            }).WithMessage("Service with Id: {PropertyValue} doesn't exist");
        }
    }
}
