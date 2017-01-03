using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public class MedioDifusionService : EntityService<MedioDifusion>, IMedioDifusionService
    {
        IUnitOfWork _unitOfWork;
        IMedioDifusionRepository _medioDifusionRepository;

        public MedioDifusionService(IUnitOfWork unitOfWork, IMedioDifusionRepository medioDifusionRepository)
            : base(unitOfWork, medioDifusionRepository)
        {
            _unitOfWork = unitOfWork;
            _medioDifusionRepository = medioDifusionRepository;
        }
   
    }
}
