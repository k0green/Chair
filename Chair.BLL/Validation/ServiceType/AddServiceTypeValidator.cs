using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ServiceType
{
    public class AddServiceTypeValidator : AbstractValidator<AddServiceTypeQuery>
    {
        private readonly ApplicationDbContext _context;
        public AddServiceTypeValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.AddServiceTypeDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.AddServiceTypeDto).MustAsync(async (dto, token) =>
            {
                var serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Name == dto.Name);

                return serviceType == null;
            }).WithMessage("service with name: {PropertyValue} exists");
            _context = context;
        }
    }
}
