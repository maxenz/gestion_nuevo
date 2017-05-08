using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class SocialServiceTypesService : EntityService<SocialServiceType>, ISocialServiceTypesService
    {
        public SocialServiceTypesService(IUnitOfWork unitOfWork, IGenericRepository<SocialServiceType> repository)
            : base(unitOfWork, repository)
        {
        }
    }
}
