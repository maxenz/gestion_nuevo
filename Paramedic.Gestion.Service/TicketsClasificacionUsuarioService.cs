using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class TicketsClasificacionUsuarioService : EntityService<TicketsClasificacionUsuario>, ITicketsClasificacionUsuarioService
    {
        IUnitOfWork _unitOfWork;
        ITicketsClasificacionUsuarioRepository _repo;

        public TicketsClasificacionUsuarioService(IUnitOfWork unitOfWork, ITicketsClasificacionUsuarioRepository repo)
            : base(unitOfWork, repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }
    }
}
