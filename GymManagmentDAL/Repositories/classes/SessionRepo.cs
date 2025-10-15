using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.classes
{
    public class SessionRepo : GenericRepo<Session>, ISessionRepo
    {
        private readonly GymDbContext _dbContext;

        public SessionRepo(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _dbContext.Sessions.Include(X => X.SessionTrainer).Include(X => X.Category).ToList();
        }

        public int GetCountOfBookSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(X => X.SessionId == sessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            var session = _dbContext.Sessions
                .Include(s => s.SessionTrainer)
                .Include(s => s.Category)
                .FirstOrDefault(s => s.Id == sessionId);
            return session;
        }
    }
}
