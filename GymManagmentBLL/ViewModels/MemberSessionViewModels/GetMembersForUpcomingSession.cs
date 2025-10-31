using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.MemberSessionViewModels
{
    public class GetMembersForUpcomingSession
    {
        public string MemberName { get; set; } = null!;
        public DateTime BookingDate { get; set; }
        public  int SessionId { get; set; }
        public int MemberId { get; set; }
    }
}
