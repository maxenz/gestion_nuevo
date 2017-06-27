using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
	public class LogsRegistrosSistemaService : EntityService<LogRegistroSistema>, ILogsRegistrosSistemaService
	{
		public LogsRegistrosSistemaService(IUnitOfWork unitOfWork,  ILogsRegistrosSistemaRepository repo)
			: base(unitOfWork, repo)
		{
		}
	}
}
