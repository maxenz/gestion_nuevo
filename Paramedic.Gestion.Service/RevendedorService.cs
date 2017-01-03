using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public class RevendedorService : EntityService<Revendedor>, IRevendedorService
    {
        IUnitOfWork _unitOfWork;
        IRevendedorRepository _revendedorRepository;

        public RevendedorService(IUnitOfWork unitOfWork, IRevendedorRepository revendedorRepository)
            : base(unitOfWork, revendedorRepository)
        {
            _unitOfWork = unitOfWork;
            _revendedorRepository = revendedorRepository;
        }
    }
}
