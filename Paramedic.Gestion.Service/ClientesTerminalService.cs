using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ClientesTerminalService : EntityService<ClientesTerminal>, IClientesTerminalService
    {
        #region Properties

        IUnitOfWork _unitOfWork;
        IClientesTerminalRepository _repo;

        #endregion

        #region Constructors

        public ClientesTerminalService(IUnitOfWork unitOfWork, IClientesTerminalRepository repo)
    : base(unitOfWork, repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }

        #endregion
    }
}
