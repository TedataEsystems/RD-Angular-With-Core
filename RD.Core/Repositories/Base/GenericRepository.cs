using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RD.Core.EFContext;
using RD.Core.Repositories.Interfaces;
using RD.Core.Uow;
using RD.Domain.Base;
using RD.Domain.Identity;

namespace RD.Core.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T>
         where T : BaseEntity
    {
        private readonly IDatabaseContext _context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(IDatabaseContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        public virtual EntityState Add(T entity)
        {
            entity.ModificationDate = DateTime.Now;
            return dbSet.Add(entity).State;
        }

        public T Get<TKey>(TKey id)
        {            
            return dbSet.Find(id);
        }

        public async Task<T> GetAsync<TKey>(TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        public T Get(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }
        public IQueryable<T> Get(params Expression<Func<T, object>>[] includes)
        {
            //DbSet<T> dbSet = UnitOfWork.Context.Set<T>();

            IQueryable<T> query = dbSet.Where(a => a.IsDeleted.Value != true).AsQueryable<T>();

            return includes.Aggregate(query, (current, include) => current.Include(include)).AsQueryable<T>();
        }
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }
        
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string include)
        {
            return FindBy(predicate).Where(c=>c.IsDeleted == false).Include(include);
        }
        
        public IQueryable<T> GetAll()
        {
            return dbSet.Where(c => c.IsDeleted == false);
        }

        public IQueryable<T> GetAll(int page, int pageCount)
        {
            var pageSize = (page - 1) * pageCount;

            return dbSet.Where(c => c.IsDeleted == false).Skip(pageSize).Take(pageCount);
        }
        
        public IQueryable<T> GetAll(string include)
        {
            return dbSet.Where(c => c.IsDeleted == false).Include(include);
        }
        
        public IQueryable<T> RawSql(string query, params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters);
        }
        
        public IQueryable<T> GetAll(string include, string include2)
        {
            return dbSet.Where(c => c.IsDeleted == false).Include(include).Include(include2);
        }
        public IQueryable<T> GetAll(string include, string include2, string include3, string include4)
        {
            return dbSet.Where(c => c.IsDeleted == false).Include(include).Include(include2);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Any(predicate);
        }

        public virtual EntityState SoftDelete(T entity)
        {
            entity.GetType().GetProperty("IsActive")?.SetValue(entity, false);
            return dbSet.Update(entity).State;
        }

        public virtual EntityState HardDelete(T entity)
        {
            return dbSet.Remove(entity).State;
        }
        public virtual EntityState Update(T entity)
        {
            //dbSet.Attach(entity);
            //var entry = _context.Entry(entity);
            //entry.State = EntityState.Modified;

            return dbSet.Update(entity).State;
        }
       

        public virtual void AddRange(List<T> entity)
        {
             dbSet.AddRange(entity);
        }
    }
}
