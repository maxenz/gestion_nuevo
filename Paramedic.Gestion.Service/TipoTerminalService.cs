using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class TipoTerminalService : EntityService<TipoTerminal>, ITipoTerminalService
    {
        IUnitOfWork _unitOfWork;
        ITipoTerminalRepository _tipoTerminalRepository;

        public TipoTerminalService(IUnitOfWork unitOfWork, ITipoTerminalRepository tipoTerminalRepository)
            : base(unitOfWork, tipoTerminalRepository)
        {
            _unitOfWork = unitOfWork;
            _tipoTerminalRepository = tipoTerminalRepository;
        }
    }
}
