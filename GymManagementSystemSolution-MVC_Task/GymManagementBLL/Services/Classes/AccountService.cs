using GymManagementBLL.Services.Intefaces;
using GymManagementBLL.ViewModels.AccountViewModel;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public ApplicationUser? VaidateUser(LoginViewModel loginViewModel)
        {
            var User = _userManager.FindByEmailAsync(loginViewModel.Email).Result;

            if (User is null) return null;

            var IsPasswordValid = _userManager.CheckPasswordAsync(User, loginViewModel.Password).Result;
            return IsPasswordValid ? User : null;   
        }
    }
}
