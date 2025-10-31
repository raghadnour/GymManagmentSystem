using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.MemberSessionViewModels
{
    public class GetMembersForOngoingSession
    {

        public string MemberName { get; set; } = null!;
        public int MemberId { get; set; }
        public bool Attendanse { get; set; } 
        public int SessionId { get; set; }

    }
}
