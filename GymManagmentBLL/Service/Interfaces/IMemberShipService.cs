using GymManagmentBLL.ViewModels.MemberShipViewModels;
using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface IMemberShipService
    {
        IEnumerable<MemberShipViewModel> GetAllMemberships();
        bool CreateMemberShip(CreateMemberShipViewModel model);
        IEnumerable<MemberSelectViewModel> GetMembersForDropDown();
        IEnumerable<PlanSelectViewModel> GetPlansForDropDown();
        bool RemoveMemberShip(int MemebrId , int PlanId);


    }
}
