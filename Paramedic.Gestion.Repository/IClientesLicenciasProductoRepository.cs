using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Repository
{
    public interface IClientesLicenciasProductoRepository : IGenericRepository<ClientesLicenciasProducto>
    {
		ClientesLicenciasProducto GetByLicenseAndProduct(int cliLicId, int productId);
	}
}
