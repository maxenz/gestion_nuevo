using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
	public class ClientesLicenciasProductosModulosHistorialRepository : GenericRepository<ClientesLicenciasProductosModulosHistorial>, IClientesLicenciasProductosModulosHistorialRepository
	{
		public ClientesLicenciasProductosModulosHistorialRepository(DbContext context) : base(context)
		{
		}
	}
}
