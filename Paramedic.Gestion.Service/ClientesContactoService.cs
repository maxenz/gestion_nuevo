using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ClientesContactoService : EntityService<ClientesContacto>, IClientesContactoService
    {
        IUnitOfWork _unitOfWork;
        IClientesContactoRepository _clientesContactoRepository;

        public ClientesContactoService(IUnitOfWork unitOfWork, IClientesContactoRepository clientesContactoRepository)
            : base(unitOfWork, clientesContactoRepository)
        {
            _unitOfWork = unitOfWork;
            _clientesContactoRepository = clientesContactoRepository;
        }
    }
}
