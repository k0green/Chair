using Chair.BLL.CQRS.Order;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Order
{
    public class RemoveOrderValidator : AbstractValidator<RemoveOrderQuery>
    {
        private readonly ApplicationDbContext _context;
        public RemoveOrderValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).MustAsync(async (id, token) =>
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

                return order != null;
            }).WithMessage("Order with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.Id).MustAsync(async (id, token) =>
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
                if (order.ClientApprove || order.ClientId != null)
                    return false;
                return true;
            }).WithMessage("You cannot delete a confirmed order");
        }
    }
}
