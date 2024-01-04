using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RD.Core.EFContext;
using RD.Core.Factory;
using RD.Core.Repositories.Base;
using RD.Core.Repositories.Interfaces;
using RD.Domain.Base;

namespace RD.Core.Uow
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private IDatabaseContext dbContext;

        private Dictionary<Type, object> repos;
        public IDatabaseContext Context { get; }
        public UnitOfWork(IDatabaseContext databaseContext)
        {
            dbContext = databaseContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity
        {
            if (repos == null)
            {
                repos = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repos.ContainsKey(type))
            {
                repos[type] = new GenericRepository<TEntity>(dbContext);
            }

            return (IGenericRepository<TEntity>)repos[type];
        }

        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            return dbContext.SaveChanges();
        }
        public async Task<int> CommitAsync()
        {
            return await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(obj: this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dbContext != null)
                {
                    dbContext.Dispose();
                    dbContext = null;
                }
            }
        }
    }
}
