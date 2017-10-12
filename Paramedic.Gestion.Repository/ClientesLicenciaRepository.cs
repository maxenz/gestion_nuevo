using Paramedic.Gestion.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Paramedic.Gestion.Repository
{
	public class ClientesLicenciaRepository : GenericRepository<ClientesLicencia>, IClientesLicenciaRepository
	{
		#region Constructors

		public DbContext Context { get; set; }

		public ClientesLicenciaRepository(DbContext context)
	: base(context)
		{
			this.Context = context;
		}

		#endregion

		#region Public Methods

		public ClientesLicencia GetById(int id)
		{
			return _dbset.Include(x => x.ClientesLicenciasProductos).FirstOrDefault(x => x.Id == id);
			
		}

		public ClientesLicencia GetByLicenseNumber(string license)
		{
			return _dbset.FirstOrDefault
			(
				x =>
					x.Licencia.Serial.Trim().ToUpper() == license.Trim().ToUpper()
				);
		}

		#endregion
	}
}
