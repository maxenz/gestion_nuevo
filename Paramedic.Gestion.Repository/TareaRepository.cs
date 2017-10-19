using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
	public class TareaRepository : GenericRepository<Tarea>, ITareaRepository
	{
		public TareaRepository(DbContext context)
			: base(context)
		{

		}
	}
}
