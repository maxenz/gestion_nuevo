using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
    public class SocialServiceTypesRepository : GenericRepository<SocialServiceType>, ISocialServiceTypesRepository
    {
        public SocialServiceTypesRepository(DbContext context) : base(context)
        {
        }
    }
}
