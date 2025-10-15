using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
    public class Session : BaseEntity
    {
        #region Properties
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        #endregion
        #region Relationships
        #region Session - Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        #endregion
        #region Session - Trainer
        public int TrainerId { get; set; }
        public Trainer SessionTrainer { get; set; } = null!;
        #endregion
        #region Session - MemberSession
        public ICollection<MemberSession> Members { get; set; } = null!;
        #endregion
        #endregion
    }
}
