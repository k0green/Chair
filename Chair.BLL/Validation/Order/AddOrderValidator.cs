using Chair.BLL.CQRS.Order;
using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Order
{
    public class AddOrderValidator : AbstractValidator<AddOrderQuery>
    {
        private readonly ApplicationDbContext _context;
        public AddOrderValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.AddOrderDtos).NotNull().NotEmpty().WithMessage("{PropertyName} can't be null or empty");

            RuleFor(x => x.AddOrderDtos.Select(x=>x.ExecutorServiceId)).MustAsync(async (exId, token) =>
            {
                foreach (var item in exId)
                {
                    var executorService = _context.ExecutorServices.FirstOrDefault(x=>x.Id == item);
                    if (executorService == null)
                        return false;
                }

                return true;
            }).WithMessage("executor service with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.AddOrderDtos.Select(x => x.ClientId)).MustAsync(async (exId, token) =>
            {
                foreach (var item in exId)
                {
                    var user = _context.Users.FirstOrDefault(x => x.Id == item);
                    if (user == null)
                        return false;
                }

                return true;
            }).WithMessage("user with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.AddOrderDtos).MustAsync(async (dtos, token) =>
            {
                foreach (var item in dtos)
                {
                    var executorId = _context.ExecutorServices.Include(x => x.Executor)
                        .First(x => x.Id == item.ExecutorServiceId).Executor.UserId;
                    if (item.ClientId == executorId)
                            return false;
                }

                return true;
            }).WithMessage("You can't book your own order");
        }
    }
}
