using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Intefaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionById(int sessionId);

        bool CreateSession(CreateSessionViewModel CreateSession);

        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);

        bool UpdateSession(UpdateSessionViewModel UpdateSession , int sessionId);

        bool RemoveSession(int sessionId);

        IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown();

        IEnumerable<CategorySelectViewModel> GetCategoryForDropDown();
    }
}
