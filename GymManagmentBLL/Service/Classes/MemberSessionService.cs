using AutoMapper;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberSessionViewModels;
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
    public class MemberSessionService : IMemberSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISessionRepo _sessionRepo;
        private readonly IMemberSessionRepo _memberSessionRepo;

        public MemberSessionService(IUnitOfWork unitOfWork, IMapper mapper , ISessionRepo sessionRepo , IMemberSessionRepo memberSessionRepo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sessionRepo = sessionRepo;
            _memberSessionRepo = memberSessionRepo;
        }

        public bool BookingSession(BookingSessionViewModel model)
        {
            try
            {
                var sessions = _unitOfWork.SessionRepo.GetById(model.SessionId);
                var member = _unitOfWork.GetRepository<Member>().GetById(model.MemberId);

                if (sessions is null || member is null)
                    return false;

                bool hasActiveMembership = _unitOfWork.MemberShipRepo
                    .GetAll()
                    .Any(m => m.MemberId == model.MemberId && m.Statues == "Active");
                if (!hasActiveMembership) return false;


                int bookedCount = _unitOfWork.MemberSessionRepo
                    .GetAll()
                    .Count(ms => ms.SessionId == model.SessionId);
                if(bookedCount>sessions.Capacity) return false;


                bool alreadyBooked = _unitOfWork.MemberSessionRepo
                    .GetAll()
                    .Any(ms => ms.MemberId == model.MemberId && ms.SessionId == model.SessionId);
                if (alreadyBooked) return false;


                if (sessions.StartTime <= DateTime.Now)
                    return false;


                var booking = new MemberSession
                {
                    MemberId = model.MemberId,
                    SessionId = model.SessionId,
                    CreatedAt = DateTime.Now,
                    IsAttended = false
                };

                _unitOfWork.MemberSessionRepo.Add(booking);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }



        public IEnumerable<SessionViewModel> GetAllMemberSessions()
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

        public IEnumerable<MemberSelectViewModel> GetMemberSelectViewModels()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            return members.Select(x => new MemberSelectViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();

        }

        public IEnumerable<GetMembersForOngoingSession> GetMembersForOngoingSessions(int sessionId)
        {
            var membersRepo = _unitOfWork.MemberSessionRepo.GetAllMemberSessionMembers(sessionId);
            if(membersRepo == null || !membersRepo.Any()) return Enumerable.Empty<GetMembersForOngoingSession>();

            var members = membersRepo.Select(X => new GetMembersForOngoingSession
            {
                MemberName = X.Member.Name,
                SessionId = X.Session.Id,
                Attendanse = X.IsAttended,
                MemberId = X.Member.Id,

            });
            return members;
        }

        public IEnumerable<GetMembersForUpcomingSession> GetMembersForUpcomingSessions(int sessionId)
        {
            var memberRepo=_unitOfWork.MemberSessionRepo.GetAllMemberSessionMembers(sessionId);
            if(memberRepo == null || !memberRepo.Any()) return Enumerable.Empty<GetMembersForUpcomingSession>();
            var members = memberRepo.Select(X => new GetMembersForUpcomingSession
            {
                MemberName = X.Member.Name,
                SessionId = X.Session.Id,
                BookingDate = X.CreatedAt,
                MemberId= X.Member.Id,
            });
            return members;
        }

        public bool MarkAsAttended(int sessionId, int memberId)
        {
            var repo = _unitOfWork.MemberSessionRepo.GetAllMemberSessionMembers(sessionId);

            var memberSession = repo
                .FirstOrDefault(x => x.Session.Id == sessionId && x.Member.Id == memberId);

            if (memberSession == null )
                return false;

            memberSession.IsAttended =!memberSession.IsAttended;
            memberSession.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<MemberSession>().Update(memberSession);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool RemoveBookingSession(int sessionId , int memberId)
        {
            try
            {
                var booking = _unitOfWork.MemberSessionRepo
                                         .GetAll()
                                         .FirstOrDefault(b => b.MemberId == memberId && b.SessionId == sessionId);

                if (booking == null) return false;
                var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
                if (session == null)return false;
                if (session.StartTime <= DateTime.Now) return false;
                

                _unitOfWork.MemberSessionRepo.Delete(booking);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch(Exception ex)
            { 
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
