using GymManagmentBLL.Service.Classes;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberSessionViewModels;
using GymManagmentBLL.ViewModels.MemberShipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
    public class MemberSessionController : Controller
    {
        private readonly IMemberSessionService _memberSessionService;

        public MemberSessionController(IMemberSessionService memberSessionService)
        {
            _memberSessionService = memberSessionService;
        }
        public ActionResult Index()
        {
            var memberSession = _memberSessionService.GetAllMemberSessions();
            return View(memberSession);
        }

        public ActionResult GetMembersForSessionUpcoming(int sessionId)
        {
            if (sessionId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = sessionId;
            var members = _memberSessionService.GetMembersForUpcomingSessions(sessionId);
            return View(members);
        }

        public ActionResult GetMembersForSessionOngoing(int sessionId)
        {
            if (sessionId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Seeion Id";
                return RedirectToAction(nameof(Index));
            }
            var members = _memberSessionService.GetMembersForOngoingSessions(sessionId);
            return View(members);
        }
        [HttpPost]
        public ActionResult MarkAsAttended(int sessionId, int memberId)
        {
            if (sessionId <= 0 || memberId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid";
            }
            var res= _memberSessionService.MarkAsAttended(sessionId, memberId);
            return RedirectToAction(nameof(GetMembersForSessionOngoing), new { sessionId });

        }

        public ActionResult Create(int sessionId)
        {
            ViewBag.SessionId = sessionId;
            LoadMemberDropDown();
            return View();
        }

        [HttpPost]
        public ActionResult Create(BookingSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadMemberDropDown();
                ViewBag.SessionId = model.SessionId; 
                return View(model);
            }

            var res = _memberSessionService.BookingSession(model);

            if (!res)
            {
                ModelState.AddModelError("", "Failed to create booking. Please try again.");
                LoadMemberDropDown();
                ViewBag.SessionId = model.SessionId;
                return View(model);
            }

            TempData["SuccessMessage"] = "Booking created successfully!";
            return RedirectToAction(nameof(GetMembersForSessionUpcoming), new { sessionId = model.SessionId });
        }
        [HttpPost]
        public ActionResult Cancel(int sessionId ,int memberId)
        {
            if (memberId < 0 || sessionId < 0)
            {
                TempData["ErrorMessage"] = "Invalid";
            }

            var result = _memberSessionService.RemoveBookingSession(sessionId,memberId);
            if (result)
            {
                TempData["SuccessMessage"] = "Membership deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete membership. Please try again.";
            }

            return RedirectToAction(nameof(Index));

        }



        #region Helper
        private void LoadMemberDropDown()
        {
            var member = _memberSessionService.GetMemberSelectViewModels();
            ViewBag.Members = new SelectList(member, "Id", "Name");
        }
        #endregion
    }
}
