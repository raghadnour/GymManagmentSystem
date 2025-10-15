using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
    public class Membership :BaseEntity
    {
        //StartDate - CreatedAt
        public DateTime EndDate { get; set; }
        public string Statues { 
            get {
                if (DateTime.Now > EndDate) 
                    return "Expired"; 
                else 
                    return "Active";
            }
        }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
    }
}
