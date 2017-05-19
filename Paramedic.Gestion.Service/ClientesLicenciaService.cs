using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ClientesLicenciaService : EntityService<ClientesLicencia>, IClientesLicenciaService
    {
        #region Properties

        IUnitOfWork _unitOfWork;
        IClientesLicenciaRepository _clientesLicenciaRepository;

        #endregion

        #region Constructors

        public ClientesLicenciaService(IUnitOfWork unitOfWork, IClientesLicenciaRepository clientesLicenciaRepository)
    : base(unitOfWork, clientesLicenciaRepository)
        {
            _unitOfWork = unitOfWork;
            _clientesLicenciaRepository = clientesLicenciaRepository;
        }

        #endregion

        #region Public Methods

        public ClientesLicencia GetById(int id)
        {
            return _clientesLicenciaRepository.GetById(id);
        }

        #endregion
    }
}
