using Microsoft.EntityFrameworkCore;
using RD.Core.Uow;
using RD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SampleProject.Services
{
    public class RDDataServices : IRDDataServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public RDDataServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(RDData entity)
        {
            bool result = false;

            try
            {
                if (entity != null)
                {
                    //bissnus
                    
                    var repository = _unitOfWork.GetRepository<RDData>();
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
       
        public IQueryable<RDData> Get(Expression<Func<RDData, bool>> predicate)
        {
            var repository = _unitOfWork.GetRepository<RDData>();
            return repository.FindBy(predicate);
        }
        public IQueryable<RDData> Get(params Expression<Func<RDData, object>>[] includes)
        {
            var repository = _unitOfWork.GetRepository<RDData>();
            return (IQueryable<RDData>)repository.Get(includes);

        }
        public IQueryable<RDData> Get(Expression<Func<RDData, bool>> predicate, params Expression<Func<RDData, object>>[] includes)
        {
            var repository = _unitOfWork.GetRepository<RDData>();
            return (IQueryable<RDData>)repository.Get(predicate, includes);

        }

        public IList<RDData> GetAll()
        {
            var repository = _unitOfWork.GetRepository<RDData>();
            return repository.GetAll().OrderByDescending(x => x.Id).ToList();
        }


        public IQueryable<RDData> GetAllIQueryable()
        {
            var repository = _unitOfWork.GetRepository<RDData>();
            return repository.GetAll();
        }

        public RDData GetById(int entityId)
        {
            var repository = _unitOfWork.GetRepository<RDData>();
            return repository.Get(entityId);
        }
        public bool Update(RDData entityItem)
        {
            bool result = false;

            try
            {
                if (entityItem != null)
                {
                    var repository = _unitOfWork.GetRepository<RDData>();
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
        public IQueryable<RDData> GetAll(string include)
        {
            var repository = _unitOfWork.GetRepository<RDData>();
            return repository.GetAll(include);
        }
        public IQueryable<RDData> GetAll(string include, string include2)
        {
            var repository = _unitOfWork.GetRepository<RDData>();
            return repository.GetAll(include , include2);
        }

        public IQueryable<RDData> GetAll(string include, string include2, string include3, string include4)
        {
            var repository = _unitOfWork.GetRepository<RDData>();
            return repository.GetAll(include, include2, include3, include4);
        }
       
        public bool AddRange(List<RDData> entity)
        {
            bool result = false;

            try
            {
                if (entity != null)
                {
                    //bissnus

                    var repository = _unitOfWork.GetRepository<RDData>();
                    repository.AddRange(entity);
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

    }
}
