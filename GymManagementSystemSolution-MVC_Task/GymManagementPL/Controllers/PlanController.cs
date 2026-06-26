using GymManagementBLL.Services.Intefaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanServices _planServices;

        public PlanController(IPlanServices planServices)
        {
            _planServices = planServices;
        }

        #region Get All Plans
        // Index
        public ActionResult Index()
        {
            var Plans = _planServices.GetAllPlans();
            return View(Plans);
        }
        #endregion

        #region Plan Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planServices.GetPlanById(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        #endregion

        #region Edit Plan

        // GET : Edit
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planServices.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // Post : Edit
        [HttpPost]
        public ActionResult Edit(int id, UpdatePlanViewModel UpdatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(UpdatedPlan);
            }

            var Result = _planServices.UpdatePlan(id, UpdatedPlan);
            if (Result)
                TempData["SuccessMessage"] = "Plan Updated";
            else
                TempData["ErrorMessage"] = "Plan Failed To Update";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Activate Plan

        [HttpPost]
        public ActionResult Activate(int id)
        {
            var result = _planServices.ToggleStatus(id);

            if (result)
                TempData["SuccessMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Failed to Change Plan Status";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}