using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public class LocalidadService : EntityService<Localidad>, ILocalidadService
    {
        IUnitOfWork _unitOfWork;
        ILocalidadRepository _localidadRepository;

        public LocalidadService(IUnitOfWork unitOfWork, ILocalidadRepository localidadRepository)
            : base(unitOfWork, localidadRepository)
        {
            _unitOfWork = unitOfWork;
            _localidadRepository = localidadRepository;
        }

        public Localidad GetById(int Id)
        {
            return _localidadRepository.GetById(Id);
        }
    }
}
