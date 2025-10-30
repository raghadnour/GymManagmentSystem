using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.AccountViewModels;
using GymManagmentDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public ApplicationUser? ValidateUser(AccountViewModel accountViewModel)
        {
            var user =  _userManager.FindByEmailAsync(accountViewModel.Email).Result;
            if (user == null) return null;
            var isPasswordValid = _userManager.CheckPasswordAsync(user, accountViewModel.Password).Result;
            return isPasswordValid ? user : null;
        }
    }
}
