using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
    public class SitioRepository : GenericRepository<Sitio>, ISitioRepository
    {
        public SitioRepository(DbContext context)
            : base(context)
        {

        }
    }
}
