using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public class ProvinciaService : EntityService<Provincia>, IProvinciaService
    {
        IUnitOfWork _unitOfWork;
        IProvinciaRepository _provinciaRepository;

        public ProvinciaService(IUnitOfWork unitOfWork, IProvinciaRepository provinciaRepository)
            : base(unitOfWork, provinciaRepository)
        {
            _unitOfWork = unitOfWork;
            _provinciaRepository = provinciaRepository;
        }
    }
}
