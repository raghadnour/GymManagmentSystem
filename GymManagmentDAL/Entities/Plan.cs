using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
    public class Plan : BaseEntity
    {
        #region Properties

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public bool IsActive { get; set; }

        #endregion
        #region Relationships
        #region Plan - Membership
        public ICollection<Membership> PlansMember { get; set; } = null!;
        #endregion
        #endregion
    }
}
