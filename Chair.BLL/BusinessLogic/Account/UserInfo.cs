using Chair.DAL.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Chair.BLL.BusinessLogic.Account
{
    public class UserInfo
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserInfo(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<string> GetCurrentUserId()
        {
            var user = await _userManager.GetUserAsync(_signInManager.Context.User);
            return user?.Id;
        }
    }
}
