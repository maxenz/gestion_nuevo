using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
    public interface IClientesGestionService : IEntityService<ClientesGestion>
    {
        IEnumerable<ClientesGestion> GetRecontactos(DateTime from, DateTime to);

        Expression<Func<ClientesGestion, bool>> GetPredicateByConditions(RecontactosControllerParametersDTO queryParameters);

        IEnumerable<ClientesGestion> GetRecontactosByPage(RecontactosControllerParametersDTO queryParameters);
    }
}
