using GymManagementBLL.ViewModels.AccountViewModel;
using GymManagementDAL.Entities;

namespace GymManagementBLL.Services.Intefaces
{
    public interface IAccountService
    {
        ApplicationUser? VaidateUser(LoginViewModel loginViewModel);
    }
}
