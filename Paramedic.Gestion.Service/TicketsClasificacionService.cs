using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public class TicketsClasificacionService : EntityService<TicketsClasificacion>, ITicketsClasificacionService
    {
        public TicketsClasificacionService(IUnitOfWork unitOfWork, ITicketsClasificacionRepository repo)
            : base(unitOfWork, repo)
        {
        }

    }
}
