using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
    public interface ILicenciasLogService : IEntityService<LicenciasLog>
    {
        IEnumerable<LicenciasLog> GetLicenciasLog(LicenciasLogControllerParametersDTO parameters);

        Expression<Func<LicenciasLog, bool>> getPredicateByConditions(LicenciasLogControllerParametersDTO parameters);
    }
}
