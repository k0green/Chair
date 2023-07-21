using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using Microsoft.EntityFrameworkCore.Query;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.ExecutorService
{
    public class LoginValidator : AbstractValidator<LoginQuery>
    {
        private readonly ApplicationDbContext _context;
        public LoginValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.LoginDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.LoginDto.Email).MustAsync(async (email, token) =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(x=> x.Email == email);

                return user != null;
            }).WithMessage("user with login: {PropertyValue} doesn't exists");
        }
    }
}
