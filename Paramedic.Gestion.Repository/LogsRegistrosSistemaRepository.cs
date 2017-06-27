using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
	public class LogsRegistrosSistemaRepository : GenericRepository<LogRegistroSistema>, ILogsRegistrosSistemaRepository
	{
		public LogsRegistrosSistemaRepository(DbContext context)
			: base(context)
		{

		}
	}
}
