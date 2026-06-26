using GymManagementBLL.ViewModels.AnalyticViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Intefaces
{
    public interface IAnalyticService
    {

        AnalyticViewModel GetAnalyticData();

    }
}
