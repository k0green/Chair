using Chair.BLL.CQRS.Order;
using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Order
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderQuery>
    {
        private readonly ApplicationDbContext _context;

        public UpdateOrderValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.UpdateOrderDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.UpdateOrderDto.ClientId).MustAsync(async (id, token) =>
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                return user == null;
            }).WithMessage("User with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.UpdateOrderDto.ExecutorServiceId).MustAsync(async (id, token) =>
            {
                var service = await _context.ExecutorServices.FirstOrDefaultAsync(x => x.Id == id);

                return service != null;
            }).WithMessage("Executor service with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.UpdateOrderDto.Id).MustAsync(async (id, token) =>
            {
                var order = await _context.Orders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                return order != null;
            }).WithMessage("Order with Id: {PropertyValue} doesn't exist");

            RuleFor(x => x.UpdateOrderDto).MustAsync(async (dto, token) =>
            {
                if (!dto.ExecutorApprove)
                {
                    if (!dto.ClientApprove)
                        return true;
                    return false;
                }

                return true;
            }).WithMessage("Нou cannot confirm the order because the executor has not confirmed it");

            RuleFor(x => x.UpdateOrderDto).MustAsync(async (dto, token) =>
            {
                    var executorId = _context.ExecutorServices.Include(x => x.Executor)
                        .First(x => x.Id == dto.ExecutorServiceId).Executor.UserId;
                    return dto.ClientId != executorId;
            }).WithMessage("You can't book your own order");
        }
    }
}
