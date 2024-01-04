using System;
using System.Threading.Tasks;
using RD.Core.EFContext;
using RD.Core.Repositories.Interfaces;
using RD.Domain.Base;

namespace RD.Core.Uow
{
    public interface IUnitOfWork
    {
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit();
        /// <returns>The number of objects in an Added, Modified, or Deleted state asynchronously</returns>
        Task<int> CommitAsync();
        /// <returns>Repository</returns>
        IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity;
        IDatabaseContext Context { get; }
    }
}
