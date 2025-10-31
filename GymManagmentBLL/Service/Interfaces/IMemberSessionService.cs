using GymManagmentBLL.ViewModels.MemberSessionViewModels;
using GymManagmentBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface IMemberSessionService
    {
        IEnumerable<SessionViewModel> GetAllMemberSessions();
        IEnumerable<GetMembersForOngoingSession> GetMembersForOngoingSessions(int sessionId);
        IEnumerable<GetMembersForUpcomingSession> GetMembersForUpcomingSessions(int sessionId);
        bool MarkAsAttended(int sessionId, int memberId);

        IEnumerable<MemberSelectViewModel> GetMemberSelectViewModels();
        bool BookingSession(BookingSessionViewModel session);

        bool RemoveBookingSession(int sessionId, int memberId);
    }
}
