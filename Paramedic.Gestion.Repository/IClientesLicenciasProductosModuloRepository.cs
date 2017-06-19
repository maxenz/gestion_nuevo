using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Repository
{
    public interface IClientesLicenciasProductosModuloRepository : IGenericRepository<ClientesLicenciasProductosModulo>
    {
		ClientesLicenciasProductosModulo GetByLicenseAndModule(int cliLicProdId, int prodModId);
	}
}
