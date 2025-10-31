using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IMemberSessionRepo : IGenericRepo<MemberSession>
    {
        IEnumerable<MemberSession> GetAllMemberSessionMembers(int sessionId);

    }
}
