using Paramedic.Gestion.Model;
using System.Data.Entity;
using System.Linq;

namespace Paramedic.Gestion.Repository
{
    public class ClientesLicenciasProductoRepository : GenericRepository<ClientesLicenciasProducto>, IClientesLicenciasProductoRepository
    {
        public ClientesLicenciasProductoRepository(DbContext context)
            : base(context)
        {

        }

		public ClientesLicenciasProducto GetByLicenseAndProduct(int cliLicId, int productId)
		{
			return _dbset.FirstOrDefault(
				x =>
					x.ProductoId == productId && x.ClientesLicenciaId == cliLicId
				);
		}
	}
}
