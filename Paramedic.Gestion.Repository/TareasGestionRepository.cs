using Paramedic.Gestion.Model;
using System.Data.Entity;


namespace Paramedic.Gestion.Repository
{
	public class TareasGestionRepository : GenericRepository<TareasGestion>, ITareasGestionRepository
	{
		public TareasGestionRepository(DbContext context)
			: base(context)
		{

		}
	}
}
