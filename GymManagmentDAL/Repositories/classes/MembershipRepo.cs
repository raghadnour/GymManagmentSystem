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
    public class MembershipRepo : GenericRepo<Membership>, IMemberShipRepo
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepo(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Membership> GetAllMembershipsWithMembersAndPlans()
        {
            return _dbContext.Memberships.Include(X=> X.Member).Include(X => X.Plan).ToList();
        }
    }
}
