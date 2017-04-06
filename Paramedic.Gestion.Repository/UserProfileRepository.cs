using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Repository
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(DbContext context)
            : base(context)
        {

        }

        public UserProfile GetById(int id)
        {
            return _dbset.Include(x => x.Emails).FirstOrDefault(x => x.Id == id);
        }
    }
}
