using RD.Core.Uow;
using RD.Domain.Entities;
using RD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RD.Services
{
    public class LogDataServices: ILogDataServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public LogDataServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(LogData entity)
        {
            bool result = false;

            try
            {
                if (entity != null)
                {
                    //bissnus

                    var repository = _unitOfWork.GetRepository<LogData>();
                    repository.Add(entity);
                    _unitOfWork.Commit();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return result;
        }
        public IQueryable<LogData> Get(Expression<Func<LogData, bool>> predicate)
        {
            var repository = _unitOfWork.GetRepository<LogData>();
            return repository.FindBy(predicate);
        }
        public IQueryable<LogData> Get(params Expression<Func<LogData, object>>[] includes)
        {
            var repository = _unitOfWork.GetRepository<LogData>();
            return (IQueryable<LogData>)repository.Get(includes);

        }
        public IQueryable<LogData> Get(Expression<Func<LogData, bool>> predicate, params Expression<Func<LogData, object>>[] includes)
        {
            var repository = _unitOfWork.GetRepository<LogData>();
            return (IQueryable<LogData>)repository.Get(predicate, includes);

        }

        public IList<LogData> GetAll()
        {
            var repository = _unitOfWork.GetRepository<LogData>();
            return repository.GetAll().OrderByDescending(x => x.Id).ToList();
        }
        public IQueryable<LogData> GetAllIQueryable()
        {
            var repository = _unitOfWork.GetRepository<LogData>();
            return repository.GetAll();
        }
        public LogData GetById(int entityId)
        {
            var repository = _unitOfWork.GetRepository<LogData>();
            return repository.Get(entityId);
        }
        public bool Update(LogData entityItem)
        {
            bool result = false;

            try
            {
                if (entityItem != null)
                {
                    var repository = _unitOfWork.GetRepository<LogData>();
                    repository.Update(entityItem);
                    _unitOfWork.Commit();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return result;
                //throw;
            }

            return result;
        }
        public IQueryable<LogData> GetAll(string include)
        {
            var repository = _unitOfWork.GetRepository<LogData>();
            return repository.GetAll(include);
        }
        public IQueryable<LogData> GetAll(string include, string include2)
        {
            var repository = _unitOfWork.GetRepository<LogData>();
            return repository.GetAll(include, include2);
        }
        public IQueryable<LogData> GetAll(string include, string include2, string include3, string include4)
        {
            var repository = _unitOfWork.GetRepository<LogData>();
            return repository.GetAll(include, include2, include3, include4);
        }
        public void Delete(LogData entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }
    }
}
