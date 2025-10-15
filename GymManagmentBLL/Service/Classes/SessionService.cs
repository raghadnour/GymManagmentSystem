using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.SessionViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createSessionViewModel)
        {
            try
            {
                if (!IsTrainerExists(createSessionViewModel.TrainerId) ||
              !IsCategoryExists(createSessionViewModel.CategoryId) ||
              !IsValidTimeRange(createSessionViewModel.StartTime, createSessionViewModel.EndTime) ||
               createSessionViewModel.Capacity < 0 || createSessionViewModel.Capacity > 25)
                {

                    return false;
                }
                var session = _mapper.Map<CreateSessionViewModel, Session>(createSessionViewModel);
                _unitOfWork.GetRepository<Session>().Add(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;
            }

        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepo.GetAllSessionsWithTrainerAndCategory();
            if (sessions == null || !sessions.Any())
                return Enumerable.Empty<SessionViewModel>();
            var sessionViewModels = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var svm in sessionViewModels)
            {
                var session = sessions.FirstOrDefault(s => s.Id == svm.Id);
                if (session != null)
                {
                    svm.AvailableSlots = session.Capacity - _unitOfWork.SessionRepo.GetCountOfBookSlots(session.Id);
                }
            }
            return sessionViewModels;
        }

        public SessionViewModel? GetSessionById(int id)
        {
            var session = _unitOfWork.SessionRepo.GetSessionWithTrainerAndCategory(id);
            if (session is null)
                return null;
            var sessionViewModel = _mapper.Map<Session, SessionViewModel>(session);
            
            sessionViewModel.AvailableSlots = session.Capacity - _unitOfWork.SessionRepo.GetCountOfBookSlots(session.Id);
            return sessionViewModel;
        }

        public UpdateSessionViewModel? GetSessionForUpdate(int id)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(id);
            if (session is null || !IsSessionAvailable(session))
                return null;
            var updateSessionViewModel = _mapper.Map<Session, UpdateSessionViewModel>(session);
            
            return updateSessionViewModel;
        }

        public bool UpdateSession(int id, UpdateSessionViewModel updateSessionViewModel)
        {
            try
            {
                var session = _unitOfWork.GetRepository<Session>().GetById(id);
                if (session is null || !IsSessionAvailable(session) ||
                    !IsTrainerExists(updateSessionViewModel.TrainerId) ||
                    !IsValidTimeRange(updateSessionViewModel.StartTime, updateSessionViewModel.EndTime))
                {
                    return false;
                }
                var updatedSession = _mapper.Map(updateSessionViewModel, session);
                updatedSession.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Session>().Update(updatedSession);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        public bool RemoveSession(int id)
        {
            try
            {
                var session = _unitOfWork.GetRepository<Session>().GetById(id);
                if (session is null || !IsSessionAvailableToDelete(session))
                    return false;
                _unitOfWork.GetRepository<Session>().Delete(session);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        #region Helper
        bool IsTrainerExists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }
        bool IsCategoryExists(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }
        bool IsValidTimeRange(DateTime startTime, DateTime endTime)
        {
            return startTime < endTime;
        }

        bool IsSessionAvailable(Session session)
        {
            if(session == null)
                return false;
            if(session.StartTime < DateTime.Now || session.EndTime < DateTime.Now)
                return false;
            if(session.Capacity <= _unitOfWork.SessionRepo.GetCountOfBookSlots(session.Id))
                return false;
            return true;
        }
        bool IsSessionAvailableToDelete(Session session)
        {
            if (session == null)
                return false;
            if (session.StartTime < DateTime.Now && session.EndTime > DateTime.Now)
                return false;
            if(session.StartTime > DateTime.Now)
                return false;
            if (session.Capacity <= _unitOfWork.SessionRepo.GetCountOfBookSlots(session.Id))
                return false;
            return true;
        }




        #endregion
    }
}
