using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.AccountViewModels;
using GymManagmentDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var user = _accountService.ValidateUser(viewModel);
            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(viewModel);
            }
            var Result = _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false).Result;
            if (Result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (Result.IsLockedOut)
            {
                ModelState.AddModelError("InvalidLogin", "Account is Locked Out");
                return View(viewModel);
            }
            if (Result.IsNotAllowed)
            {
                ModelState.AddModelError("InvalidLogin", "You are not allowed to login");
                return View(viewModel);
            }

            return View(viewModel);
        }

        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
