using GymManagementBLL.Services.Intefaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberServices _memberServices;

        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }

        #region Get All Members
        public ActionResult Index()
        {
            var members = _memberServices.GetAllMembers();
            return View(members);
        }
        #endregion

        #region Get Member Data

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberServices.GetMemberDetails(id);

            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var HealthRecord = _memberServices.GetMemberHealthRecordDetails(id);
            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(HealthRecord); 
        }

        #endregion

        #region Create Member

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreatedMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid" , "Check Data And Missing Fields");
                return View(nameof(Create));
            }

            bool Result = _memberServices.CreateMember(CreatedMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create , Check Phone And Email";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Member

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            { 
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberServices.GetMemberToUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id , MemberToUpdateViewModel MemberToEdit)
        {
            if (!ModelState.IsValid)
                return View(MemberToEdit);

            var Result = _memberServices.UpdateMemberDetails(id , MemberToEdit);
            if (Result)
                TempData["SuccessMessgge"] = "Member Updated Successfully";
            else
                TempData["ErrorMessage"] = "Member Failed To Updated";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Member

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberServices.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberName = Member.Name;
            ViewBag.MemberId = id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = _memberServices.RemoveMember(id);
            if (Result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Member Can Not Delete";
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
