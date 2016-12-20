using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public class PaisService : EntityService<Pais>, IPaisService
    {
        IUnitOfWork _unitOfWork;
        IPaisRepository _paisRepository;

        public PaisService(IUnitOfWork unitOfWork, IPaisRepository paisRepository)
            : base(unitOfWork, paisRepository)
        {
            _unitOfWork = unitOfWork;
            _paisRepository = paisRepository;
        }


        public Pais GetById(int Id)
        {
            return _paisRepository.GetById(Id);
        }
    }
}
