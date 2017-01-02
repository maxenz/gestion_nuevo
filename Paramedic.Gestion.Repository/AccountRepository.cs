using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Repository
{
    public class AccountRepository : GenericRepository<UserProfile>, IAccountRepository
    {
        public AccountRepository(DbContext context)
            : base(context)
        {

        }

    }
}
