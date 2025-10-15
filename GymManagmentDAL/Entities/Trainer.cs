using GymManagmentDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
    public class Trainer : GymUser
    {
        // HireDate = UpdatedAt from BaseEntity
        public Specialities Specialities { get; set; }

        #region Relationships
        #region Trainer - Session
        public ICollection<Session> TrainerSessions { get; set; } = null!;
        #endregion
        #endregion
    }
}
