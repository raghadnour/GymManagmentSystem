using GymManagmentBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int id);
        bool UpdatePlan(int id, PlanUpdateViewModel plan);
        PlanUpdateViewModel? GetPlanToUpdate(int id);
        bool Activate(int id);


    }
}
