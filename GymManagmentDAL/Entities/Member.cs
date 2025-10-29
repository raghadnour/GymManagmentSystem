using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
    public class Member : GymUser
    {
        public string Photo { get; set; } = null!;
        #region Relationships

        #region Member - HealthRecord
        public HealthRecord HealthRecord { get; set; } = null!;

        #endregion
        #region Member - Membership
        public ICollection<Membership> Membership { get; set; } = null!;
        #endregion
        #region Member - MemberSession
        public ICollection<MemberSession> Sessions { get; set; } = null!;
        #endregion
        #endregion
    }
}
