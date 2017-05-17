using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ClientesLicenciaService : EntityService<ClientesLicencia>, IClientesLicenciaService
    {
        #region Constructors

        public ClientesLicenciaService(IUnitOfWork unitOfWork, IClientesLicenciaRepository clientesLicenciaRepository)
    : base(unitOfWork, clientesLicenciaRepository)
        {
        }

        #endregion

    }
}
