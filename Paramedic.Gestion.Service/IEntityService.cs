using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{

    public interface IEntityService<T> : IService
        where T : BaseEntity
    {
        void Create(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Update(T entity);
        IEnumerable<T> FindByPage(Expression<Func<T, bool>> whereExp, string orderExpression, int pageSize, int page = 1);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> whereExp);
        int GetCount(Expression<Func<T, bool>> whereExp);
    }
}
