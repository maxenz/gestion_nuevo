using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;


namespace Paramedic.Gestion.Service
{
    public class SitioService : EntityService<Sitio>, ISitioService
    {
        public SitioService(IUnitOfWork unitOfWork, ISitioRepository sitioRepository)
            : base(unitOfWork, sitioRepository)
        {
        }
    }
}
