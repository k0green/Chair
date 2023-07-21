using Chair.BLL.Dto.Account;
using Chair.DAL.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Chair.BLL.BusinessLogic.Account
{
    public class AccountBusinessLogic : IAccountBusinessLogic
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountBusinessLogic(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task Register(RegisterDto model)
        {
            var user = new User { Email = model.Email, UserName = model.Email, PhoneNumber = model.Phone };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                await _signInManager.SignInAsync(user, false);
            else
                throw new InvalidOperationException("Registration failed");
        }

        public async Task Login(LoginDto model)
        {
            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
                throw new InvalidOperationException("Login failed");
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
