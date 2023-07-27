using Chair.BLL.CQRS.Order;
using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Order
{
    public class CancelOrderValidator : AbstractValidator<CancelOrderQuery>
    {
        private readonly ApplicationDbContext _context;

        public CancelOrderValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.OrderId).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.OrderId).MustAsync(async (id, token) =>
            {
                var order = await _context.Orders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                return order != null;
            }).WithMessage("Order with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.OrderId).MustAsync(async (id, token) =>
            {
                return true;
                //var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
                //if (order.ClientId != CurrentUser.Id) return true;
                //return DateTime.Now.AddHours(6) <= order.StarDate;
                //TODO нада сделать
            }).WithMessage("You can't cancel the order because there are more than 6 hours left before it");
        }
    }
}
