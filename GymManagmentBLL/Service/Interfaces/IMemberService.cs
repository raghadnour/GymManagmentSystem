using GymManagmentBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel createMemberViewModel);
        MemberViewModel? GetMemberDetails(int id);

        HealthRecordViewModel? GetHealthRecordDetails(int id);
        bool UpdateMember(int id, MemberUpdateViewModel updateMemberViewModel);
        MemberUpdateViewModel? GetMemberForUpdate(int id);
        bool RemoveMember(int id);
    }
}
