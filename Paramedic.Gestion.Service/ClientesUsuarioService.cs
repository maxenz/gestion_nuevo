using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ClientesUsuarioService : EntityService<ClientesUsuario>, IClientesUsuarioService
    {
        IUnitOfWork _unitOfWork;
        IClientesUsuarioRepository _repo;

        public ClientesUsuarioService(IUnitOfWork unitOfWork, IClientesUsuarioRepository repo)
            : base(unitOfWork, repo)
        {
        }
    }
}
