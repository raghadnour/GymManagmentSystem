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
    public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : BaseEntity
    {
        private readonly GymDbContext _dbContext;

        public GenericRepo(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(TEntity entity)
        {
            _dbContext.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? Condition = null)
        {
            if(Condition != null)
                return _dbContext.Set<TEntity>().Where(Condition).ToList();

            return _dbContext.Set<TEntity>().ToList();

        }

        public TEntity? GetById(int id) => _dbContext.Set<TEntity>().Find(id);

        public void Update(TEntity entity)
        {
            _dbContext.Update(entity);
            
        }
    }
}
