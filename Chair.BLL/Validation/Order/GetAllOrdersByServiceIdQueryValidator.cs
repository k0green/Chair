using Chair.BLL.CQRS.Order;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Order
{
    public class GetAllOrdersByServiceIdQueryValidator : AbstractValidator<GetAllOrdersByServiceIdQuery>
    {
        private readonly ApplicationDbContext _context;
        public GetAllOrdersByServiceIdQueryValidator(ApplicationDbContext context)
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
