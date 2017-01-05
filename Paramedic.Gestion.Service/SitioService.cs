using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;


namespace Paramedic.Gestion.Service
{
    public class SitioService : EntityService<Sitio>, ISitioService
    {
        IUnitOfWork _unitOfWork;
        ISitioRepository _sitioRepository;

        public SitioService(IUnitOfWork unitOfWork, ISitioRepository sitioRepository)
            : base(unitOfWork, sitioRepository)
        {
        }
    }
}
