using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ClientesLicenciasProductoService : EntityService<ClientesLicenciasProducto>, IClientesLicenciasProductoService
    {
        IUnitOfWork _unitOfWork;
        IClientesLicenciasProductoRepository _repo;

        public ClientesLicenciasProductoService(IUnitOfWork unitOfWork, IClientesLicenciasProductoRepository repo)
            : base(unitOfWork, repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }
    }
}
