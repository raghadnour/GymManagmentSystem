using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface ISessionRepo : IGenericRepo<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();
        int GetCountOfBookSlots(int sessionId);
        Session? GetSessionWithTrainerAndCategory(int sessionId);

    }
}
