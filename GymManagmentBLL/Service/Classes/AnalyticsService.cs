using GymManagementSystemBLL.ViewModels.AnalyticsViewModels;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AnalyticsViewModel GetAnalyticsData()
        {
            var Sessions = _unitOfWork.SessionRepo.GetAll();
            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<Membership>().GetAll( x => x.Statues == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = Sessions.Count(predicate: x => x.StartTime > DateTime.Now),
                OngoingSessions = Sessions.Count(predicate: x => x.StartTime <= DateTime.Now && x.EndTime >= DateTime.Now),
                CompletedSessions = Sessions.Count(predicate: x => x.EndTime < DateTime.Now)
            };
        }
    }
}
