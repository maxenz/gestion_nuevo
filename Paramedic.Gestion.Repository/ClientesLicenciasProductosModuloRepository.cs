using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
    public class ClientesLicenciasProductosModuloRepository : GenericRepository<ClientesLicenciasProductosModulo>, IClientesLicenciasProductosModuloRepository
    {
        public ClientesLicenciasProductosModuloRepository(DbContext context)
            : base(context)
        {

        }
    }
}
