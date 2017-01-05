using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Paramedic.Gestion.Repository
{

    public abstract class GenericRepository<T> : IGenericRepository<T>
          where T : BaseEntity
    {
        protected DbContext _entities;
        protected readonly IDbSet<T> _dbset;

        public GenericRepository(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable<T>();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query;
            if (predicate != null)
            {
                query = _dbset.Where(predicate).AsEnumerable();
            }
            else
            {
                query = _dbset.AsEnumerable();
            }

            return query;
        }

        public IEnumerable<T> FindByPage(Expression<Func<T, bool>> whereExp, string orderExp, int pageSize, int page = 1)
        {
            IEnumerable<T> query;

            if (whereExp != null)
            {
                query = _dbset.Where(whereExp).OrderBy(orderExp).Skip((page - 1) * pageSize).Take(pageSize);
            }
            else
            {
                query = _dbset.OrderBy(orderExp).Skip((page - 1) * pageSize).Take(pageSize);
            }

            return query;
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}
