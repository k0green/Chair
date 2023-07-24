using Chair.BLL.CQRS.ExecutorProfile;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorProfile
{
    public class GetAllProfilesByServiceTypeIdValidator : AbstractValidator<GetAllProfilesByServiceTypeIdQuery>
    {
        private readonly ApplicationDbContext _context;
        public GetAllProfilesByServiceTypeIdValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.ServiceTypeId).MustAsync(async (id, token) =>
            {
                var executor = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Id == id);

                return executor != null;
            }).WithMessage("Service type with Id: {PropertyValue} doesn't exist");
        }
    }
}
