using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.PlanViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || !Plans.Any()) return [];
            var planViewModels = Plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsActive = p.IsActive,
                DurationDays = p.DurationDays
            });
            return planViewModels;
        }

        public PlanViewModel? GetPlanById(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is not null)
            {
                PlanViewModel planViewModel = new PlanViewModel
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    Description = plan.Description,
                    Price = plan.Price,
                    IsActive = plan.IsActive,
                    DurationDays = plan.DurationDays
                };
                return planViewModel;
            }
            return null;
        }

        public PlanUpdateViewModel? GetPlanForUpdate(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null || HasMembership(id) || plan.IsActive == false) return null;
            
                PlanUpdateViewModel planViewModel = new PlanUpdateViewModel
                {
                    Name = plan.Name,
                    Description = plan.Description,
                    Price = plan.Price,
                    DurationDays = plan.DurationDays
                };
                return planViewModel;
        }

        public bool TogglePlanStatus(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if(plan is null || HasMembership(id)) return false;
            try
            {
                plan.IsActive = !plan.IsActive;
                plan.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePlan(int id, PlanUpdateViewModel plan)
        {
            var PlanUpdate = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (PlanUpdate is null || HasMembership(id) || PlanUpdate.IsActive == false) return false;
            try
            {
                PlanUpdate.Price = plan.Price;
                PlanUpdate.Description = plan.Description;
                PlanUpdate.DurationDays = plan.DurationDays;
                PlanUpdate.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Plan>().Update(PlanUpdate);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper
        bool HasMembership(int id)
        {
            var memberships = _unitOfWork.GetRepository<Membership>().GetAll(m => m.PlanId == id && m.Statues == "Active");
            return memberships.Any();

        }
        #endregion
    }
}
