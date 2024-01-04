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
    public class UsersServices : IUsersServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(Users entity)
        {
            bool result = false;

            try
            {
                if (entity != null)
                {
                    //bissnus
                    
                    var repository = _unitOfWork.GetRepository<Users>();
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
       
        public IQueryable<Users> Get(Expression<Func<Users, bool>> predicate)
        {
            var repository = _unitOfWork.GetRepository<Users>();
            return repository.FindBy(predicate);
        }
        public IQueryable<Users> Get(params Expression<Func<Users, object>>[] includes)
        {
            var repository = _unitOfWork.GetRepository<Users>();
            return (IQueryable<Users>)repository.Get(includes);

        }
        public IQueryable<Users> Get(Expression<Func<Users, bool>> predicate, params Expression<Func<Users, object>>[] includes)
        {
            var repository = _unitOfWork.GetRepository<Users>();
            return (IQueryable<Users>)repository.Get(predicate, includes);

        }

        public IList<Users> GetAll()
        {
            var repository = _unitOfWork.GetRepository<Users>();
            return repository.GetAll().OrderByDescending(x => x.Id).ToList();
        }


        public IQueryable<Users> GetAllIQueryable()
        {
            var repository = _unitOfWork.GetRepository<Users>();
            return repository.GetAll();
        }

        public Users GetById(int entityId)
        {
            var repository = _unitOfWork.GetRepository<Users>();
            return repository.Get(entityId);
        }
        public bool Update(Users entityItem)
        {
            bool result = false;

            try
            {
                if (entityItem != null)
                {
                    var repository = _unitOfWork.GetRepository<Users>();
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
        public IQueryable<Users> GetAll(string include)
        {
            var repository = _unitOfWork.GetRepository<Users>();
            return repository.GetAll(include);
        }
        public IQueryable<Users> GetAll(string include, string include2)
        {
            var repository = _unitOfWork.GetRepository<Users>();
            return repository.GetAll(include , include2);
        }

        public IQueryable<Users> GetAll(string include, string include2, string include3, string include4)
        {
            var repository = _unitOfWork.GetRepository<Users>();
            return repository.GetAll(include, include2, include3, include4);
        }
        public void AddRange(List<Users> entity)
        {
            _unitOfWork.Context.Set<Users>().AddRange(entity);
            _unitOfWork.Commit();
        }
        public void Delete(Users entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }

    }
}
