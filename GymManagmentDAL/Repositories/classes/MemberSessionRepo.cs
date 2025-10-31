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
    public class MemberSessionRepo : GenericRepo<MemberSession>, IMemberSessionRepo
    {
        private readonly GymDbContext _dbContext;

        public MemberSessionRepo(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<MemberSession> GetAllMemberSessionMembers(int sessionId)
        {
            return _dbContext.MemberSessions.Include(X=>X.Member)
                .Include(X=>X.Session)
                .Where(ms => ms.SessionId == sessionId)
                .ToList();
        }
    }
}
