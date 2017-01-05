using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class LicenciaService : EntityService<Licencia>, ILicenciaService
    {
        IUnitOfWork _unitOfWork;
        ILicenciaRepository _licenciaRepository;

        public LicenciaService(IUnitOfWork unitOfWork, ILicenciaRepository licenciaRepository)
            : base(unitOfWork, licenciaRepository)
        {
            _unitOfWork = unitOfWork;
            _licenciaRepository = licenciaRepository;
        }

    }
}
