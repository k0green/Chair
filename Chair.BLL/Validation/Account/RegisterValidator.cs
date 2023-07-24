using Chair.BLL.CQRS.ServiceType;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Chair.BLL.Validation.Account
{
    public class RegisterValidator : AbstractValidator<RegisterQuery>
    {
        private readonly ApplicationDbContext _context;
        public RegisterValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.RegisterDto).NotNull().WithMessage("{PropertyName} can't be null");

            RuleFor(x => x.RegisterDto.Email).NotEmpty().WithMessage("Email can't be empty")
                .EmailAddress().WithMessage("Invalid email address format");

            RuleFor(x => x.RegisterDto).MustAsync(async (dto, token) =>
            {
                return dto.ConfirmPassword == dto.Password;
            }).WithMessage("confirm password incorrect");

            RuleFor(x => x.RegisterDto.Email).MustAsync(async (email, token) =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

                return user == null;
            }).WithMessage("user with login: {PropertyValue} exists");

            RuleFor(x => x.RegisterDto.Phone).MustAsync(async (phone, token) =>
            {
                var pattern = @"^\+375\d{9}$";
                return Regex.IsMatch(phone, pattern);
            }).WithMessage("phone number: {PropertyValue} incorrect");

        }
    }
}
