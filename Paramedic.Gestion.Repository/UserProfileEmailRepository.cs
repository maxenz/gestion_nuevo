using Paramedic.Gestion.Model;
using System.Data.Entity;


namespace Paramedic.Gestion.Repository
{
    public class UserProfileEmailRepository : GenericRepository<UserProfileEmail>, IUserProfileEmailRepository
    {
        public UserProfileEmailRepository(DbContext context)
            : base(context)
        {

        }
    }
}
