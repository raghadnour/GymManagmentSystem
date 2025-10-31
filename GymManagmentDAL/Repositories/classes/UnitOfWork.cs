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
    public class UnitOfWork : IUnitOfWork
    {
        public ISessionRepo SessionRepo { get; }
        public IMemberShipRepo MemberShipRepo { get; }
        public IMemberSessionRepo MemberSessionRepo { get; }


        private readonly Dictionary<string, object> repositories = [];
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext,
            ISessionRepo sessionRepository , IMemberShipRepo memberShipRepo , IMemberSessionRepo memberSessionRepo)
        {
            _dbContext = dbContext;
            SessionRepo = sessionRepository;
            MemberShipRepo = memberShipRepo;
            MemberSessionRepo = memberSessionRepo;
        }


        public IGenericRepo<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var typeName = typeof(TEntity).Name;
            if (repositories.TryGetValue(typeName, out object? value))
                return (IGenericRepo<TEntity>)value;
            var Repo = new GenericRepo<TEntity>(_dbContext);
            repositories[typeName] = Repo;
            return Repo;
        }

        public int SaveChanges()
        => _dbContext.SaveChanges();
    }
}
