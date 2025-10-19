using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IGenericRepo<TEntity> where TEntity : BaseEntity 
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity? GetById(int id);
        IEnumerable<TEntity> GetAll(Func<TEntity,bool>? Condition =null);
    }
}
