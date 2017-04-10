using Paramedic.Gestion.Model;
using System.Data.Entity;
using System.Linq;


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
