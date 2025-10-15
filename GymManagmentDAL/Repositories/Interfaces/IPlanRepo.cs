using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IPlanRepo
    {
        public IEnumerable<Plan> GetAll();
        public Plan? GetById(int id);
        public int Update(Plan plan);
    }
}
