using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IMemberShipRepo :IGenericRepo<Membership>
    {
        IEnumerable<Membership> GetAllMembershipsWithMembersAndPlans();

    }
}
