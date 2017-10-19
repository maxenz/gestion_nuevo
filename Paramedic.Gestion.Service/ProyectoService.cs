using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
	public class ProyectoService : EntityService<Proyecto>, IProyectoService
	{
		IUnitOfWork _unitOfWork;
		IProyectoRepository _repo;

		public ProyectoService(IUnitOfWork unitOfWork, IProyectoRepository repo)
			: base(unitOfWork, repo)
		{
		}
	}
}
