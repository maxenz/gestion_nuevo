using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Service
{
    public interface IClientesLicenciaService : IEntityService<ClientesLicencia>
    {
        ClientesLicencia GetById(int id);

		ClientesLicenciasProductosModulosHistorial GetAddonHistorial(string license, int prodModId);

		List<ClientesLicenciasProductosModulo> GetProductosModulosForAddon(string licencia);
	}
}
