using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Intefaces;
using GymManagementBLL.ViewModels.BookingViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class BookingController(IBookingService _bookingService) : Controller
    {

        #region GetAll Sessions
        public IActionResult Index()
        {
            var sessions = _bookingService.GetAllSessionsWithTrainersAndCategory();
            return View(sessions);
        }

        #endregion

        #region Upcoming Sessions

        public IActionResult UpcomingSessions(int id)
        {
            var Members = _bookingService.GetAllMembersForSession(id);
            return View(Members);
        }
        #endregion

        #region Ongoing Sessions

        public IActionResult OngoingSessions(int id)
        {
            var Members = _bookingService.GetAllMembersForSession(id);
            return View(Members);
        }
        #endregion

        #region Create

        public IActionResult Create(int id)
        {
            var Members = _bookingService.GetMemberForDropDown(id);
            var MemberSelectList = new SelectList(Members, "Id", "Name");

            ViewBag.Members = MemberSelectList;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBookingViewModel model)
        {
            var result = _bookingService.CreateBooking(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Booking Created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Booking.";
            }

            return RedirectToAction(nameof(OngoingSessions), new { id = model.SessionId });
        }

        #endregion

        #region Attended

        [HttpPost]
        public IActionResult Attended(MemberAttendOrCancelViewModel model)
        {
            var result = _bookingService.MemberAttended(model);

            if (result)
                TempData["SuccessMessage"] = "Member attended successfully";
            else
                TempData["ErrorMessage"] = "Member attendance can't be marked";

            return RedirectToAction(nameof(OngoingSessions), new { id = model.SessionId });
        }

        #endregion

        #region Canceled

        [HttpPost]
        public IActionResult Cancel(MemberAttendOrCancelViewModel model)
        {
            var result = _bookingService.CancelBooking(model);

            if (result)
                TempData["SuccessMessage"] = "Booking cancelled successfully";
            else
                TempData["ErrorMessage"] = "Booking can't be cancelled";
            return RedirectToAction(nameof(UpcomingSessions), new { id = model.SessionId });
        } 
        #endregion
    }
}
