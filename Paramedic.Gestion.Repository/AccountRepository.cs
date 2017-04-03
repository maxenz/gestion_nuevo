using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Paramedic.Gestion.Repository
{
    public class AccountRepository : GenericRepository<UserProfile>, IAccountRepository
    {
        public AccountRepository(DbContext context)
            : base(context)
        {

        }

        public override IEnumerable<UserProfile> FindByPage(Expression<Func<UserProfile, bool>> whereExp, string orderExp, int pageSize, int page = 1)
        {
            IEnumerable<UserProfile> query;

            if (whereExp != null)
            {
                query = _dbset.Where(whereExp).Include(x => x.Emails).OrderBy(orderExp).Skip((page - 1) * pageSize).Take(pageSize);
            }
            else
            {
                query = _dbset.Include(x => x.Emails).OrderBy(orderExp).Skip((page - 1) * pageSize).Take(pageSize);
            }

            return query;

        }



    }
}
