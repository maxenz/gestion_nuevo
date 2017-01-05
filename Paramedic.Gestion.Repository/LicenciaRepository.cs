using Paramedic.Gestion.Model;
using System.Data.Entity;


namespace Paramedic.Gestion.Repository
{
    public class LicenciaRepository : GenericRepository<Licencia>, ILicenciaRepository
    {
        public LicenciaRepository(DbContext context)
            : base(context)
        {

        }
    }
}
