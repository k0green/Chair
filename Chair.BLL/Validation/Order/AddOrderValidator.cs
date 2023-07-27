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
                return exId.Select(item => _context.ExecutorServices.FirstOrDefault(x => x.Id == item)).All(executorService => executorService != null);
            }).WithMessage("executor service with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.AddOrderDtos.Select(x => x.ClientId)).MustAsync(async (exId, token) =>
            {
                return exId.Select(item => _context.Users.FirstOrDefault(x => x.Id == item)).All(user => user != null);
            }).WithMessage("user with id: {PropertyValue} doesn't exists");

            RuleFor(x => x.AddOrderDtos).MustAsync(async (dtos, token) =>
            {
                return !(from item in dtos
                    let executorId = _context.ExecutorServices.Include(x => x.Executor)
                        .First(x => x.Id == item.ExecutorServiceId)
                        .Executor.UserId
                    where item.ClientId == executorId
                    select item).Any();
            }).WithMessage("You can't book your own order");
        }
    }
}
