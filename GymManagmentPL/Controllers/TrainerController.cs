using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagmentBLL.Service.Classes;
using GymManagmentBLL.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        #region GetAllTrainers
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        #endregion
        #region GetTrainerDetails
        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id.";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        #endregion
        #region CreateTrainer
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTrainer(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }
            var result = _trainerService.CreateTrainer(model);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create , Phone Number Or Email already exists";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion
        #region EditTranier
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id.";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id,TrainerToUpdateViewModel updateTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Fields");
                return View("Edit", updateTrainer);
            }
            bool Result = _trainerService.UpdateTrainerDetails(id,updateTrainer);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = ("UpdateFailed", "Failed to update Trainer. Please try again.");
                
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id.";
                return RedirectToAction(nameof(Index));
            }
            var member = _trainerService.GetTrainerDetails(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            return View();

        }
        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id.";
                return RedirectToAction(nameof(Index));
            }
            bool Result = _trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Delete.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
