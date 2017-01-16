using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ClientesTerminalService : EntityService<ClientesTerminal>, IClientesTerminalService
    {
        IUnitOfWork _unitOfWork;
        IClientesTerminalRepository _repo;

        public ClientesTerminalService(IUnitOfWork unitOfWork, IClientesTerminalRepository repo)
            : base(unitOfWork, repo)
        {
        }
    }
}
