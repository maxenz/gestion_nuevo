using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
    public class SocialServicesRepository : GenericRepository<SocialService>, ISocialServicesRepository
    {
        public SocialServicesRepository(DbContext context) : base(context)
        {
        }
    }
}
