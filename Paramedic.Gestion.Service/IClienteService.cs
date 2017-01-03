using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
    public interface IClienteService : IEntityService<Cliente>
    {
        IEnumerable<Cliente> GetClientsByType(ClientControllerParametersDTO parameters);

        Expression<Func<Cliente, bool>> getPredicateByConditions(ClientControllerParametersDTO parameters);

    }
}
