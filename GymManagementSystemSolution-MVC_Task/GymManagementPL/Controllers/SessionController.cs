using GymManagementBLL.Services.Intefaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Get All Sessions
        public ActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }
        #endregion

        #region Session Details

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID.";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }

        #endregion

        #region Create Session

        public ActionResult Create()
        {
            LoadDropDownsForCategories();
            LoadDropDownsForTrainers();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel CreatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForCategories();
                LoadDropDownsForTrainers();
                return View(CreatedSession);
            }

            var result = _sessionService.CreateSession(CreatedSession);

            if (result)
            {
                TempData["SuccessMessage"] = "Session Created";
                return RedirectToAction(actionName: nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Session";
                LoadDropDownsForCategories();
                LoadDropDownsForTrainers();
                return View(CreatedSession);
            }
        }

        #endregion

        #region Edit Session

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionToUpdate(id);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownsForTrainers();
            return View(session);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute]int id , UpdateSessionViewModel UpdatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForTrainers();
                return View(UpdatedSession);
            }
            var result = _sessionService.UpdateSession(UpdatedSession , id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Updated";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Updated";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Session

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = session.Id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Deleted";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Session";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region HelperMethods

        private void LoadDropDownsForCategories()
        {
            var Categories = _sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }

        private void LoadDropDownsForTrainers()
        {
            var Trainers = _sessionService.GetTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }

        #endregion
    }
}
