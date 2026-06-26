using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Intefaces
{
    public interface IPlanServices
    {
        IEnumerable<PlanViewModel> GetAllPlans();

        PlanViewModel? GetPlanById(int planId);

        UpdatePlanViewModel? GetPlanToUpdate(int PlanId);

        bool UpdatePlan(int planId , UpdatePlanViewModel updatedPlan);

        bool ToggleStatus(int PlanId);

    }
}
