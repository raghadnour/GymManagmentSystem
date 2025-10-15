using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.classes
{
    public class PlanRepo :IPlanRepo
    {
        private readonly GymDbContext _context;

        public PlanRepo(GymDbContext dbcontext)
        {
            _context = dbcontext;
        }
       
        public IEnumerable<Plan> GetAll() => _context.Plans.ToList();


        public Plan? GetById(int id) => _context.Plans.Find(id);

        public int Update(Plan plan)
        {
            _context.Plans.Update(plan);
            return _context.SaveChanges();
        }
    }
}
