using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public ISessionRepo SessionRepo { get; }
        public IMemberShipRepo MemberShipRepo { get; }
        public IMemberSessionRepo MemberSessionRepo { get; }
        IGenericRepo<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        int SaveChanges();

    }
}
