using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Intefaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MembershipController(IMembershipService _membershipService) : Controller
    {
        #region GetAll Memberships
        public IActionResult Index()
        {
            var Memeberships = _membershipService.GetAllMemberships();
            return View(Memeberships);
        }
        #endregion

        #region Create Membership
        public IActionResult Create()
        {
            LoadDropDownsForMembers();
            LoadDropDownsForPlans();
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(CreateMembershipViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Result = _membershipService.CreateMembership(model);
                if (Result)
                {
                    TempData["SuccessMessage"] = "Membership Created Successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Membership Cannot be Created";
                    return RedirectToAction(nameof(Index));
                }
            }
            LoadDropDownsForMembers();
            LoadDropDownsForPlans();
            return View(model);
        }

        #endregion

        #region Cancel

        public IActionResult Cancel(int id)
        {
            var Result = _membershipService.DeleteMembership(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Membership Deleted Successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Membership Cannot be Deleted";
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region HelperMethods

        public void LoadDropDownsForMembers()
        {
            var Members = _membershipService.GetMembersForDropDown();
            ViewBag.Members = new SelectList(Members, "Id", "Name");
        }

        public void LoadDropDownsForPlans()
        {
            var Plans = _membershipService.GetPlansForDropDown();
            ViewBag.Plans = new SelectList(Plans, "Id", "Name");
        }

        #endregion

    }
}
