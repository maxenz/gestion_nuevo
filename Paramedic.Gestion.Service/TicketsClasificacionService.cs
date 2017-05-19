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
        IUnitOfWork _unitOfWork;
        ITicketsClasificacionRepository _repo;

        public TicketsClasificacionService(IUnitOfWork unitOfWork, ITicketsClasificacionRepository repo)
            : base(unitOfWork, repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }

        public TicketsClasificacion GetById(int id)
        {
            return _repo.GetById(id);
        }

    }
}
