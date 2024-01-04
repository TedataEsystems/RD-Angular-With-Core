using RD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RD.Services
{
    public interface ILogDataServices
    {
        bool Add(LogData entity);
        bool Update(LogData entity);
        IQueryable<LogData> Get(params Expression<Func<LogData, object>>[] includes);
        IQueryable<LogData> Get(Expression<Func<LogData, bool>> predicate, params Expression<Func<LogData, object>>[] includes);
        IQueryable<LogData> Get(Expression<Func<LogData, bool>> predicate);
        IList<LogData> GetAll();
        IQueryable<LogData> GetAll(string include);
        IQueryable<LogData> GetAll(string include, string include2);
        IQueryable<LogData> GetAll(string include, string include2, string include3, string include4);
        IQueryable<LogData> GetAllIQueryable();
        LogData GetById(int entityId);
        void Delete(LogData entity);
    }
}
