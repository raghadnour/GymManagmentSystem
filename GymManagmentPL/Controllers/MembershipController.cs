using GymManagmentBLL.Service.Classes;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberShipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
        private readonly IMemberShipService _membershipService;

        public MembershipController(IMemberShipService membershipService)
        {
            _membershipService = membershipService;
        }
        public ActionResult Index()
        {
            var membeshis = _membershipService.GetAllMemberships();
            return View(membeshis);
        }

        public ActionResult Create()
        {
            LoadMembersDropDown();
            LoadPlansForDropDown();
            return View();

        }
        [HttpPost]
        public ActionResult Create(CreateMemberShipViewModel memberShipViewModel)
        {
            if (!ModelState.IsValid)
            {

                LoadMembersDropDown();
                LoadPlansForDropDown();
                return View();
            }
            var res=_membershipService.CreateMemberShip(memberShipViewModel);
            if (!res)
            {
                ModelState.AddModelError("", "Failed to create Membership.");
                LoadMembersDropDown();
                LoadPlansForDropDown();
                return View(memberShipViewModel);
            }
            else
            {
                TempData["SuccessMessage"] = "Membership created successfully!";
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public ActionResult Cancel(int MemberId ,int PlanId)
        {
            if (MemberId < 0 || PlanId < 0)
            {
                TempData["ErrorMessage"] = "Invalid";
            }

            var result = _membershipService.RemoveMemberShip(MemberId, PlanId);
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
        private void LoadMembersDropDown()
        {
            var members = _membershipService.GetMembersForDropDown();
            ViewBag.Members = new SelectList(members, "Id", "Name");
        }
        private void LoadPlansForDropDown()
        {
            var plans = _membershipService.GetPlansForDropDown();
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }
        #endregion
    }
}
