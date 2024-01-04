using RD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SampleProject.Services
{ 
    public interface IUsersServices
    {
        bool Add(Users entity);
        void AddRange(List<Users> entity);

        bool Update(Users entity);
        IQueryable<Users> Get(params Expression<Func<Users, object>>[] includes);
        IQueryable<Users> Get(Expression<Func<Users, bool>> predicate, params Expression<Func<Users, object>>[] includes);
        IQueryable<Users> Get(Expression<Func<Users, bool>> predicate);

        IList<Users> GetAll();
        IQueryable<Users> GetAll(string include);
        IQueryable<Users> GetAll(string include, string include2);
        IQueryable<Users> GetAll(string include, string include2, string include3, string include4);

        IQueryable<Users> GetAllIQueryable();

        Users GetById(int entityId);
        void Delete(Users entity);

    }

}
