using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class RemoveExecutorServiceValidator : AbstractValidator<RemoveExecutorServiceQuery>
    {
        private readonly ApplicationDbContext _context;
        public RemoveExecutorServiceValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).MustAsync(async (id, token) =>
            {
                var executorService = await _context.ExecutorServices.FirstOrDefaultAsync(x => x.Id == id);

                return executorService != null;
            }).WithMessage("Executor service with Id: {PropertyValue} doesn't exist");
        }
    }
}
