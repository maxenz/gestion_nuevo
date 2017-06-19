using Paramedic.Gestion.Model;
using System.Data.Entity;
using System.Linq;

namespace Paramedic.Gestion.Repository
{
	public class ClientesLicenciasProductosModuloRepository : GenericRepository<ClientesLicenciasProductosModulo>, IClientesLicenciasProductosModuloRepository
	{
		public ClientesLicenciasProductosModuloRepository(DbContext context)
			: base(context)
		{

		}

		public ClientesLicenciasProductosModulo GetByLicenseAndModule(int cliLicProdId, int prodModId)
		{
			return _dbset.FirstOrDefault(
					x => x.ClientesLicenciasProductoId == cliLicProdId && x.ProductosModuloId == prodModId
				);
		}

	}
}
