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
        void Update(T entity);
        IEnumerable<T> FindByPage(Expression<Func<T, bool>> whereExp, Expression<Func<T, dynamic>> orderExp, int pageSize, int page = 1);
    }
}
