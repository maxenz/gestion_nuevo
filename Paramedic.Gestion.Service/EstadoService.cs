using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class EstadoService : EntityService<Estado>, IEstadoService
    {
        IUnitOfWork _unitOfWork;
        IEstadoRepository _estadoRepository;

        public EstadoService(IUnitOfWork unitOfWork, IEstadoRepository estadoRepository)
            : base(unitOfWork, estadoRepository)
        {
        }
    }
}
