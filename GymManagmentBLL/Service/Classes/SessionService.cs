﻿using AutoMapper;
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
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepo.GetAllSessionsWithTrainerAndCategory().OrderByDescending(X => X.StartTime);

            if (sessions == null || !sessions.Any()) return Enumerable.Empty<SessionViewModel>();

            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepo.GetCountOfBookSlots(session.Id);
            }
            return MappedSessions;

        }
        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = _unitOfWork.SessionRepo.GetSessionWithTrainerAndCategory(sessionId);

            if (session == null)
                return null;

            var MappedSession = _mapper.Map<Session, SessionViewModel>(session);
            MappedSession.AvailableSlots = MappedSession.Capacity - _unitOfWork.SessionRepo.GetCountOfBookSlots(session.Id);
            return MappedSession;
        }
        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);

            if (!IsSessionValidForUpdating(session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(session);
        }
        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<Session>();

                if (!IsTrainerExists(createSession.TrainerId)) return false;
                if (!IsCategoryExists(createSession.CategoryId)) return false;
                if (!IsValidDateRange(createSession.StartTime, createSession.EndTime)) return false;
                var sessionEntity = _mapper.Map<Session>(createSession);
                sessionEntity.CreatedAt = DateTime.Now;
                repo.Add(sessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateSession(int id, UpdateSessionViewModel updateSession)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<Session>();
                var session = repo.GetById(id);

                if (!IsSessionValidForUpdating(session!)) return false;
                if (!IsTrainerExists(updateSession.TrainerId)) return false;
                if (!IsValidDateRange(updateSession.StartTime, updateSession.EndTime)) return false;

                _mapper.Map(updateSession, session);
                session!.UpdatedAt = DateTime.Now;

                repo.Update(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveSession(int sessionId)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<Session>();
                var session = repo.GetById(sessionId);

                if (!IsSessionValidForRemoving(session!)) return false;

                repo.Delete(session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods
        private bool IsSessionValidForUpdating(Session session)
        {
            // Only future sessions with no bookings
            return session.StartTime > DateTime.Now &&
           _unitOfWork.SessionRepo.GetCountOfBookSlots(session.Id) == 0;
        }
        private bool IsSessionValidForRemoving(Session session)
        {
            //  Only completed sessions with no bookings
            return session.EndTime < DateTime.Now &&
                   _unitOfWork.SessionRepo.GetCountOfBookSlots(session.Id) == 0;
        }
        private bool IsTrainerExists(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer >().GetById(id);
            return trainer is null ? false : true;
        }
        private bool IsCategoryExists(int id)
        {
            var category = _unitOfWork.GetRepository<Category >().GetById(id);
            return category is null ? false : true;
        }
        private bool IsValidDateRange(DateTime StartDate, DateTime EndDate)
        {
            return EndDate > StartDate && StartDate > DateTime.Now;
        }

        #endregion
    }
}
