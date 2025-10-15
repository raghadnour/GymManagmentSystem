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
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(GymDbContext context, ISessionRepo sessionRepo)
        {
            _dbContext = context;
            SessionRepo = sessionRepo;
        }

        public ISessionRepo SessionRepo { get; }

        public IGenericRepo<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);

            if (_repositories.TryGetValue(EntityType, out var Repo)) return (IGenericRepo<TEntity>)Repo;

            var NewRepo = new GenericRepo<TEntity>(_dbContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
