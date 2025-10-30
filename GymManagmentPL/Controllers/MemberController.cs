using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        #region GetAllMembers
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        #endregion
        #region Get Member Details
        public ActionResult MemberDetails(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id.";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);

            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }
        #endregion
        #region Get Member HealthRecord Details
        public ActionResult MemberHealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Health Record Id.";
                return RedirectToAction(nameof(Index));
            }
            var healthRecord = _memberService.GetHealthRecordDetails(id);
            if (healthRecord == null)
            {
                TempData["ErrorMessage"] = "Health Record not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
        #endregion
        #region CreateMember
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {

                ModelState.AddModelError("DataMissed", "Check Missing Fields");
                return View("Create", createMember);
            }

            bool Result = _memberService.CreateMember(createMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create , Phone Number Or Email already exists";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion
        #region EditMember
        public ActionResult MemberEdit(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id.";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberForUpdate(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id,MemberUpdateViewModel updateMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Fields");
                return View( updateMember);
            }
            bool Result = _memberService.UpdateMember(id,updateMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Update , Phone Number Or Email already exists";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region DeleteMember
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id.";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();

        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            bool Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Delete.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
