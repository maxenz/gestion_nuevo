using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Service
{
    public interface IClientesLicenciaService : IEntityService<ClientesLicencia>
    {
        ClientesLicencia GetById(int id);

		ProductosModulosIntento GetAddonIntento(string license, int prodModId);

	}
}
