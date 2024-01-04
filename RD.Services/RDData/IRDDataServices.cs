using RD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SampleProject.Services
{ 
    public interface IRDDataServices
    {
        bool Add(RDData entity);
        bool AddRange(List<RDData> entity);

        bool Update(RDData entity);
        IQueryable<RDData> Get(params Expression<Func<RDData, object>>[] includes);
        IQueryable<RDData> Get(Expression<Func<RDData, bool>> predicate, params Expression<Func<RDData, object>>[] includes);
        IQueryable<RDData> Get(Expression<Func<RDData, bool>> predicate);

        IList<RDData> GetAll();
        IQueryable<RDData> GetAll(string include);
        IQueryable<RDData> GetAll(string include, string include2);
        IQueryable<RDData> GetAll(string include, string include2, string include3, string include4);

        IQueryable<RDData> GetAllIQueryable();

        RDData GetById(int entityId);

    }

}
