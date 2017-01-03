using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
