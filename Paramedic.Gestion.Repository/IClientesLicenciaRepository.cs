using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Repository
{
    public interface IClientesLicenciaRepository : IGenericRepository<ClientesLicencia>
    {
        ClientesLicencia GetById(int id);

		ClientesLicencia GetByLicenseNumber(string license);

	}
}
