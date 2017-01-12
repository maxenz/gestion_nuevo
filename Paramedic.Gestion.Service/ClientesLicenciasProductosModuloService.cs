using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ClientesLicenciasProductosModuloService : EntityService<ClientesLicenciasProductosModulo>, IClientesLicenciasProductosModuloService
    {
        IUnitOfWork _unitOfWork;
        IClientesLicenciasProductosModuloRepository _repo;

        public ClientesLicenciasProductosModuloService(IUnitOfWork unitOfWork, IClientesLicenciasProductosModuloRepository repo)
            : base(unitOfWork, repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }
    }
}
