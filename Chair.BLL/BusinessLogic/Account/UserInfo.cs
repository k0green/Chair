using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chair.BLL.Dto.Account;
using Chair.DAL.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Chair.BLL.BusinessLogic.Account
{
    [Authorize]
    public class UserInfo
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfo(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetCurrentUserId()
        {
            
            var userId = _httpContextAccessor.HttpContext.Session.GetString("userId");

            if (userId != null)
                return userId;

            throw new InvalidOperationException("User not found");
        }
        
        public async Task<UserInfoDto> GetCurrentUserInfo()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("userId");
            return new UserInfoDto
            {
                Id = userId,
                Name = "userName",
                Email = "user.Email",
                Role = "roles.FirstOrDefault()"
            };
        }
        
        public async Task<UserInfoDto> GetCurrentUserInfoByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            return new UserInfoDto
            {
                Id = user.Id,
                Name = user.AccountName,
                Email = user.Email,
                Role = roles.FirstOrDefault()
            };
        }
        
        public async Task<string> GetUserIdFromToken()
        {
            var context = _httpContextAccessor.HttpContext;
            try
            {
                // Get the Authorization header from the request
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                // Check if the header is present and has the correct format
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    var token = authHeader.Substring("Bearer ".Length);

                    // Validate the token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_32_bytes_here"));

                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,  // Modify these according to your token setup
                        ValidateAudience = false,
                        // More validation parameters...

                    };

                    SecurityToken validatedToken;
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                    // Extract the user ID from the claims
                    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    return userId;
                }

                return null; // Authorization header is missing or has incorrect format
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting user ID from JWT token: {ex.Message}");
                throw;
            }
        }
    }
}
