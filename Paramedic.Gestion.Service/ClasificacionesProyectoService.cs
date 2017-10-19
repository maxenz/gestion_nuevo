using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
	public class ClasificacionesProyectoService : EntityService<ClasificacionesProyecto>, IClasificacionesProyectoService
	{
		IUnitOfWork _unitOfWork;
		IClasificacionesProyectoRepository _repo;

		public ClasificacionesProyectoService(IUnitOfWork unitOfWork, IClasificacionesProyectoRepository repo)
			: base(unitOfWork, repo)
		{
			_unitOfWork = unitOfWork;
			_repo = repo;
		}
	}
}
