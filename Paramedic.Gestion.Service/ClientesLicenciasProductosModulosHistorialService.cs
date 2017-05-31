using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
	public class ClientesLicenciasProductosModulosHistorialService : EntityService<ClientesLicenciasProductosModulosHistorial>, IClientesLicenciasProductosModulosHistorialService
	{
		IUnitOfWork _unitOfWork;
		IClientesLicenciasProductosModulosHistorialRepository _repo;

		public ClientesLicenciasProductosModulosHistorialService(IUnitOfWork unitOfWork, IClientesLicenciasProductosModulosHistorialRepository repo)
			: base(unitOfWork, repo)
		{
		}
	}
}
