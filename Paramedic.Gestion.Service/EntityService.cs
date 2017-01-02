using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Paramedic.Gestion.Service
{
    public abstract class EntityService<T> : IEntityService<T> where T : BaseEntity
    {
        IUnitOfWork _unitOfWork;
        IGenericRepository<T> _repository;

        public EntityService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }


        public virtual void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repository.Add(entity);
            _unitOfWork.Commit();
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Edit(entity);
            _unitOfWork.Commit();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Delete(entity);
            _unitOfWork.Commit();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual IEnumerable<T> FindByPage(Expression<Func<T, bool>> whereExp, string orderExp, int pageSize, int page = 1)
        {
            return _repository.FindByPage(whereExp, orderExp, pageSize, page);
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> whereExp)
        {
            return _repository.FindBy(whereExp);
        }

        public virtual int GetCount(Expression<Func<T, bool>> whereExp)
        {
            if (whereExp == null)
            {
                return _repository.GetAll().Count();
            }
            else
            {
                return FindBy(whereExp).Count();
            }

        }

    }
}
