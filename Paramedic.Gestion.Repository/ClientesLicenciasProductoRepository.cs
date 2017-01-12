using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
    public class ClientesLicenciasProductoRepository : GenericRepository<ClientesLicenciasProducto>, IClientesLicenciasProductoRepository
    {
        public ClientesLicenciasProductoRepository(DbContext context)
            : base(context)
        {

        }
    }
}
