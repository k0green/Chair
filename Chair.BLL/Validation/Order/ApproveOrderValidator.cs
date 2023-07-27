using Chair.BLL.CQRS.Order;
using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Order
{
    public class ApproveOrderValidator : AbstractValidator<ApproveOrderQuery>
    {
        private readonly ApplicationDbContext _context;

        public ApproveOrderValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.OrderId).NotNull().WithMessage("{PropertyName} can't be null");
            RuleFor(x => x.IsExecutor).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.OrderId).MustAsync(async (id, token) =>
            {
                var order = await _context.Orders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                return order != null;
            }).WithMessage("Order with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.OrderId).MustAsync(async (id, token) =>
            {
                var dto = await _context.Orders.FirstAsync(x => x.Id == id);
                if (!dto.ExecutorApprove)
                {
                    if (!dto.ClientApprove)
                        return true;
                    return false;
                }

                return true;
            }).WithMessage("You cannot confirm the order because the executor has not confirmed it");

            RuleFor(x => x.OrderId).MustAsync(async (id, token) =>
            {
                var dto = await _context.Orders.FirstAsync(x => x.Id == id);
                if(dto.ClientId == null)
                    return false;

                return true;
            }).WithMessage("You cannot confirm the order if you have not booked it");
        }
    }
}
