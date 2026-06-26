using GymManagementBLL.Services.Intefaces;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticService _analyticService;

        public HomeController(IAnalyticService analyticService)
        {
            _analyticService = analyticService;
        }
        public ActionResult Index()
        {
            var Data = _analyticService.GetAnalyticData();
            return View(Data);
        }

        
    }
}

