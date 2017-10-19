using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
	public class TareaService : EntityService<Tarea>, ITareaService
	{
		IUnitOfWork _unitOfWork;
		ITareaRepository _repo;

		public TareaService(IUnitOfWork unitOfWork, ITareaRepository repo)
			: base(unitOfWork, repo)
		{
		}
	}
}
