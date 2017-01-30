using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
    public class LicenciasLogRepository : GenericRepository<LicenciasLog>, ILicenciasLogRepository
    {
        public LicenciasLogRepository(DbContext context)
            : base(context)
        {

        }
    }

}