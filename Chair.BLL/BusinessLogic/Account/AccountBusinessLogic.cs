using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.Dto.Account;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Chair.BLL.BusinessLogic.Account
{
    public class AccountBusinessLogic : IAccountBusinessLogic
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IExecutorProfileBusinessLogic _executorProfile;

        public AccountBusinessLogic(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IExecutorProfileBusinessLogic executorProfileBusinessLogic)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _executorProfile = executorProfileBusinessLogic;
        }

        public async Task<string> Register(RegisterDto model)
        {
            var user = new User { Email = model.Email, UserName = model.Email, PhoneNumber = model.Phone, AccountName = model.Name};

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                var newUser = await _userManager.FindByEmailAsync(model.Email);
                await _executorProfile.AddAsync(new AddExecutorProfileDto()
                {
                    UserId = user.Id,
                    Name = model.Name,
                });
                return await GenerateJwtToken(user);
            }
            else
                throw new InvalidOperationException("Registration failed");
        }

        public async Task<string> Login(LoginDto model)
        {
            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
                throw new InvalidOperationException("Login failed");
            var user = await _userManager.FindByEmailAsync(model.Email);
            var token = await GenerateJwtToken(user);
            return token;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        
        public async Task<string> GenerateJwtToken(User user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                };
                
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                }
                
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_32_bytes_here"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(1);

                var token = new JwtSecurityToken(
                    "your_issuer",
                    "your_audience",
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );
                
                return new JwtSecurityTokenHandler().WriteToken(token);
            // ваш код генерации токена
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating JWT token: {ex.Message}");
                throw;
            }
        }
    }
}
