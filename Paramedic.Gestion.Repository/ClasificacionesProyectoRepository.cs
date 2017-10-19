using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
	public class ClasificacionesProyectoRepository : GenericRepository<ClasificacionesProyecto>, IClasificacionesProyectoRepository
	{
		public ClasificacionesProyectoRepository(DbContext context)
			: base(context)
		{

		}
	}
}
