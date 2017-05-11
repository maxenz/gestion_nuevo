using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class SocialServicesService : EntityService<SocialService>, ISocialServicesService
    {
        public SocialServicesService(IUnitOfWork unitOfWork, IGenericRepository<SocialService> repository)
            : base(unitOfWork, repository)
        {
        }
    }
}
