using GymManagmentBLL.ViewModels.AccountViewModels;
using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface IAccountService
    {
        ApplicationUser? ValidateUser(AccountViewModel accountViewModel);
    }
}
