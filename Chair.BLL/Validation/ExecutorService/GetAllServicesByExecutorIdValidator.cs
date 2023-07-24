using Chair.BLL.CQRS.ExecutorService;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class GetAllServicesByExecutorIdValidator : AbstractValidator<GetAllServicesByExecutorIdQuery>
    {
        private readonly ApplicationDbContext _context;
        public GetAllServicesByExecutorIdValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.ExecutorId).MustAsync(async (id, token) =>
            {
                var executor = await _context.ExecutorProfiles.FirstOrDefaultAsync(x => x.Id == id);

                return executor != null;
            }).WithMessage("Executor with Id: {PropertyValue} doesn't exist");
        }
    }
}
