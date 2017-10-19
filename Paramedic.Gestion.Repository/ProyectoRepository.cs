using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
	public class ProyectoRepository : GenericRepository<Proyecto>, IProyectoRepository
	{
		public ProyectoRepository(DbContext context)
			: base(context)
		{

		}
	}
}
